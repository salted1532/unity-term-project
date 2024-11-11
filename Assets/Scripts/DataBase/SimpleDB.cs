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

    void Start()
    {
        Rankpage.SetActive(false);
        Clearpage.SetActive(false);

        clearTime = 0;

        CreateDB();

        Displayuserdata();
    }

    void Update()
    {
        if (isClear == false)
        {
            clearTime += Time.deltaTime;

            cleatimer.text = clearTime.ToString();
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
                        userlist.text += "Rank: " + reader["rank"] + "\tID: " + reader["id"] + "\tcleartime: " + reader["cleartime"] + "초" + "\tReview: " + reader["review"] + "\tDate: " + reader["cleardate"] + "\n";
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
        Rankpage.SetActive(true);
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
                        Ranklist.text += i + " 등 " + "\tID: " + reader["id"] + "\tcleartime: " + reader["cleartime"] + "초" + "\tReview: " + reader["review"] + "\tDate: " + reader["cleardate"] + "\n";
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
        Rankpage.SetActive(true);
        Ranklist.text = "";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM userdata ORDER BY cleardate;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    int i = 1;
                    while (reader.Read())
                    {
                        Ranklist.text += "Rank: " + reader["rank"] + "\tID: " + reader["id"] + "\tcleartime: " + reader["cleartime"] + "초" + "\tReview: " + reader["review"] + "\tDate: " + reader["cleardate"] + "\n";
                        i++;
                    }

                    reader.Close();
                }
            }
            connection.Close();
        }
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
