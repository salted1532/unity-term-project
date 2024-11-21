using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using UnityEngine.UI;

public class SimpleDB : MonoBehaviour
{
    private string dbName = "URI=file:UserData.db";

    public TMP_InputField idInput;
    public TMP_InputField reviewInput;
    public TextMeshProUGUI userlist;

    public TextMeshProUGUI cleatimer;

    public float clearTime;

    public GameObject Rankpage;
    public TextMeshProUGUI Ranklist;

    public GameObject Clearpage;

    private bool isClear = false;

    private bool isAscendingOrder = true;
    private bool isAscendingRankOrder = true;

    void Start()
    {
        if (Rankpage != null)
        {
            Rankpage.SetActive(false);
        }
        if (Clearpage != null) {

            Clearpage.SetActive(false);
        }

        clearTime = 0;

        CreateDB();
        if (userlist != null)
        {
            Displayuserdata();
        }
    }

    void Update()
    {
        if (isClear == false)
        {
            clearTime += Time.deltaTime;

            if (cleatimer != null)
            {
                cleatimer.text = clearTime.ToString();
            }
        }
    }

    public void CreateDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS userdata(id VARCHAR(20), review VARCHAR(30), cleartime INT(4), cleardate VARCHAR(20), rank INT);";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void Adduserdata()
    {
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO userdata (id, review, cleartime, cleardate, rank) VALUES ('" + idInput.text + "', '" + reviewInput.text + "', '" + ((int)clearTime) + "', '" + currentDate + "', 0);";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        UpdateRanks();
        Displayuserdata();
        clearTime = 0;
    }

    public void UpdateRanks()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Order by cleartime ascending to assign ranks
                command.CommandText = "SELECT id FROM userdata ORDER BY cleartime;";
                List<string> ids = new List<string>();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ids.Add(reader["id"].ToString());
                    }
                    reader.Close();
                }

                // Update each user's rank based on their order in the list
                int rank = 1;
                foreach (var id in ids)
                {
                    command.CommandText = $"UPDATE userdata SET rank = {rank} WHERE id = '{id}';";
                    command.ExecuteNonQuery();
                    rank++;
                }
            }
            connection.Close();
        }
    }

    public void Displayuserdata()
    {

        userlist.text = "";


        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM userdata ORDER BY rank;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userlist.text += reader["rank"] + " �� " + "\tID: " + reader["id"] + "\tcleartime: " + reader["cleartime"] + "��" + "\tReview: " + reader["review"] + "\tDate: " + reader["cleardate"] + "\n";
                    }

                    reader.Close();
                }
            }
            connection.Close();
        }
    }

    public void Clearuserdata()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DROP TABLE IF EXISTS userdata;";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
        CreateDB();
        Displayuserdata();
    }

    public void RankpageOn()
    {
        if (Rankpage != null)
        {
            Rankpage.SetActive(true);
        }
        Ranklist.text = "";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM userdata ORDER BY rank;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    int i = 1;
                    while (reader.Read())
                    {
                        Ranklist.text += reader["rank"] + " �� " + "\tID: " + reader["id"] + "\tcleartime: " + reader["cleartime"] + "��" + "\tReview: " + reader["review"] + "\tDate: " + reader["cleardate"] + "\n";
                        i++;
                    }

                    reader.Close();
                }
            }
            connection.Close();
        }
    }

    public void DateSelectOn()
    {
        if (Rankpage != null)
        {
            Rankpage.SetActive(true);
        }

        Ranklist.text = "";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // ���� ������ ����Ͽ� ������ �������� ����
                if (isAscendingOrder)
                {
                    command.CommandText = "SELECT * FROM userdata ORDER BY cleardate ASC;";
                }
                else
                {
                    command.CommandText = "SELECT * FROM userdata ORDER BY cleardate DESC;";
                }

                using (IDataReader reader = command.ExecuteReader())
                {
                    int i = 1;
                    while (reader.Read())
                    {
                        Ranklist.text += reader["rank"] + " �� " +
                                            "\tID: " + reader["id"] +
                                            "\tcleartime: " + reader["cleartime"] + "��" +
                                            "\tReview: " + reader["review"] +
                                            "\tDate: " + reader["cleardate"] + "\n";
                        i++;
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        // ���� ������ ���
        isAscendingOrder = !isAscendingOrder;
    }

    public void Searchid()
    {
        Ranklist.text = ""; // ���� ���� �ʱ�ȭ

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Ư�� ID�� �˻��ϴ� ����
                command.CommandText = "SELECT * FROM userdata WHERE id = @id ORDER BY rank;";
                command.Parameters.AddWithValue("@id", idInput.text);

                using (IDataReader reader = command.ExecuteReader())
                {
                    int i = 1; // ������ ���� ī����
                    while (reader.Read())
                    {
                        // Ranklist�� �˻��� ������ �߰�
                        Ranklist.text += reader["rank"] + " �� " + "\tID: " + reader["id"] + "\tcleartime: " + reader["cleartime"] + "��" + "\tReview: " + reader["review"] + "\tDate: " + reader["cleardate"] + "\n";
                        i++;
                    }

                    // �˻� ����� ���� �� �޽��� ���
                    if (i == 1)
                    {
                        Ranklist.text = "�˻� ����� �����ϴ�.";
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }
    }

    public void RankSelect()
    {
        if (Rankpage != null)
        {
            Rankpage.SetActive(true);
        }
        Ranklist.text = "";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // ��� ���� ������ ����Ͽ� ���� ����
                if (isAscendingRankOrder)
                {
                    command.CommandText = "SELECT * FROM userdata ORDER BY rank ASC;";
                }
                else
                {
                    command.CommandText = "SELECT * FROM userdata ORDER BY rank DESC;";
                }

                using (IDataReader reader = command.ExecuteReader())
                {
                    int i = 1;
                    while (reader.Read())
                    {
                        Ranklist.text += reader["rank"] + " �� " +
                                         "\tID: " + reader["id"] +
                                         "\tcleartime: " + reader["cleartime"] + "��" +
                                         "\tReview: " + reader["review"] +
                                         "\tDate: " + reader["cleardate"] + "\n";
                        i++;
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        // ���� ������ ���
        isAscendingRankOrder = !isAscendingRankOrder;
    }

    public void RankpageOff()
    {
        Rankpage.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Clearpage.SetActive(true);
            isClear = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
