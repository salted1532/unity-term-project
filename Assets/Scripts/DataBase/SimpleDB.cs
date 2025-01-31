using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public float countdownTime = 1f; // 타이머 시간 (초)

    private bool isTimerRunning = false; // 타이머 상태 확인 변수

    public GameObject Player;

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

        if (Input.GetKey(KeyCode.C))
        {
            if (Input.GetKey(KeyCode.L))
            {
                if (Input.GetKey(KeyCode.E))
                {
                    Debug.Log("승리!");
                    Clearpage.SetActive(true);
                    isClear = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Player.GetComponent<PlayerController>().SetController(false);
                    Time.timeScale = 0;
                }
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
                command.CommandText = "CREATE TABLE IF NOT EXISTS userdata(id VARCHAR(20) PRIMARY KEY, review VARCHAR(500), cleartime INT, cleardate VARCHAR(20), rank INT);";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void Adduserdata()
    {
        // 입력값이 비어있거나 공백으로만 이루어진 경우 메서드 종료
        if (string.IsNullOrWhiteSpace(idInput.text))
        {
            Debug.Log("ID 입력값이 비어있거나 공백입니다.");
            clearTime = 0;

            Time.timeScale = 1;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Main Menu");
            return;
        }

        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                // 기존 데이터가 있으면 업데이트, 없으면 삽입
                command.CommandText =
                    "INSERT INTO userdata (id, review, cleartime, cleardate, rank) " +
                    "VALUES (@id, @review, @cleartime, @cleardate, @rank) " +
                    "ON CONFLICT(id) DO UPDATE SET review = excluded.review, cleartime = excluded.cleartime, cleardate = excluded.cleardate, rank = excluded.rank;";

                command.Parameters.AddWithValue("@id", idInput.text);
                command.Parameters.AddWithValue("@review", reviewInput.text);
                command.Parameters.AddWithValue("@cleartime", (int)clearTime);
                command.Parameters.AddWithValue("@cleardate", currentDate);
                command.Parameters.AddWithValue("@rank", 0);

                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        UpdateRanks();
        clearTime = 0;

        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Main Menu");
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
                        userlist.text += reader["rank"] + " 등 " + " ID: " + reader["id"] + " cleartime: " + reader["cleartime"] + "초" + " Review: " + reader["review"] + " Date: " + reader["cleardate"] + "\n";
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
                        Ranklist.text += reader["rank"] + " 등 " +
                                          " 아이디: " + reader["id"] +
                                          "\t클리어 시간: " + reader["cleartime"] + "초" +
                                          "\t날짜: " + reader["cleardate"] + "\n" +
                                          "리뷰: " + reader["review"] + "\n";
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
                // 정렬 순서를 토글하여 쿼리를 동적으로 설정
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
                        Ranklist.text += reader["rank"] + " 등 " +
                                          " 아이디: " + reader["id"] +
                                          "\t클리어 시간: " + reader["cleartime"] + "초" +
                                          "\t날짜: " + reader["cleardate"] + "\n" +
                                          "리뷰: " + reader["review"] + "\n";
                        i++;
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        // 정렬 순서를 토글
        isAscendingOrder = !isAscendingOrder;
    }

    public void Searchid()
    {
        Ranklist.text = ""; // 이전 내용 초기화

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // 특정 ID로 검색하는 쿼리
                command.CommandText = "SELECT * FROM userdata WHERE id = @id ORDER BY rank;";
                command.Parameters.AddWithValue("@id", idInput.text);

                using (IDataReader reader = command.ExecuteReader())
                {
                    int i = 1; // 순위를 위한 카운터
                    while (reader.Read())
                    {
                        // Ranklist에 검색된 데이터 추가
                        Ranklist.text += reader["rank"] + " 등 " +
                                          " 아이디: " + reader["id"] +
                                          "\t클리어 시간: " + reader["cleartime"] + "초" +
                                          "\t날짜: " + reader["cleardate"] + "\n" +
                                          "리뷰: " + reader["review"] + "\n";
                        i++;
                    }

                    // 검색 결과가 없을 때 메시지 출력
                    if (i == 1)
                    {
                        Ranklist.text = "검색 결과가 없습니다.";
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
                // 등수 정렬 순서를 토글하여 쿼리 설정
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
                        Ranklist.text += reader["rank"] + " 등 " +
                                          " 아이디: " + reader["id"] +
                                          "\t클리어 시간: " + reader["cleartime"] + "초" +
                                          "\t날짜: " + reader["cleardate"] + "\n" +
                                          "리뷰: " + reader["review"] + "\n";
                        i++;
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        // 정렬 순서를 토글
        isAscendingRankOrder = !isAscendingRankOrder;
    }

    public void ClearTimeSelect()
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
                // 등수 정렬 순서를 토글하여 쿼리 설정
                if (isAscendingRankOrder)
                {
                    command.CommandText = "SELECT * FROM userdata ORDER BY cleartime ASC;";
                }
                else
                {
                    command.CommandText = "SELECT * FROM userdata ORDER BY cleartime DESC;";
                }

                using (IDataReader reader = command.ExecuteReader())
                {
                    int i = 1;
                    while (reader.Read())
                    {
                        Ranklist.text += reader["rank"] + " 등 " +
                                          " 아이디: " + reader["id"] +
                                          "\t클리어 시간: " + reader["cleartime"] + "초" +
                                          "\t날짜: " + reader["cleardate"] + "\n" +
                                          "리뷰: " + reader["review"] + "\n";
                        i++;
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        // 정렬 순서를 토글
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
            if(Player != null)
            {
                Clearpage.SetActive(true);
                isClear = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Player.GetComponent<PlayerController>().SetController(false);
                Time.timeScale = 0;
            }
        }
    }
}
