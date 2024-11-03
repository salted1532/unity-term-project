using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEditor.MemoryProfiler;
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

    // Start is called before the first frame update
    void Start()
    {
        Rankpage.SetActive(false);
        Clearpage.SetActive(false);

        clearTime = 0;

        CreateDB();

        Displayuserdata();
    }

    // Update is called once per frame
    void Update()
    {
        if(isClear == false)
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
                command.CommandText = "CREATE TABLE IF NOT EXISTS userdata( id VARCHAR(20), review VARCHAR(30), cleartime INT(4));";
                command.ExecuteNonQuery();

            }
            connection.Close();
        }
    }

    public void Adduserdata()
    {
        using (var connection = new SqliteConnection(dbName)) 
        { 
            connection.Open();
            using (var command = connection.CreateCommand())
            {

                command.CommandText = "INSERT INTO userdata (id, review, cleartime) VALUES ('" + idInput.text + "', '" + reviewInput.text + "', '" + ((int)clearTime) + "');";

                command.ExecuteNonQuery();

            }
            connection.Close ();
        }
        Displayuserdata();
        clearTime = 0;
    }

    public void Displayuserdata()
    {
        userlist.text = "";

        using ( var  connection = new SqliteConnection(dbName))
        {
            connection.Open(); 

            using (var command = connection.CreateCommand())
            {
                command.CommandText = " SELECT * FROM userdata;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userlist.text += "ID: " + reader["id"] + "\tcleartime: " + reader["cleartime"] + "초" + "\tReview: " + reader["review"] + "\n";
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
                command.CommandText = " SELECT * FROM userdata ORDER BY cleartime;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    int i = 1;
                    while (reader.Read())
                    {
                        Ranklist.text +=  i + " 등 "+ "ID: " + reader["id"] + "\tcleartime: " + reader["cleartime"] + "초" + "\tReview: " + reader["review"] + "\n";
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
        // 플레이어가 충돌할 때 문 열기
        if (other.gameObject.CompareTag("Player"))
        {
            Clearpage.SetActive(true);
            isClear = true;
        }
    }
}
