using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : MonoBehaviour
{

    public List<int> Enemyleft = new List<int>();
    public List<GameObject> stage1EnemyObj = new List<GameObject>();
    public List<GameObject> stage2EnemyObj = new List<GameObject>();
    public List<GameObject> stage3EnemyObj = new List<GameObject>();
    public List<GameObject> stage4EnemyObj = new List<GameObject>();

    public bool Stage1 = false, Stage2 = false, Stage3 = false, Stage4 = false;

    public GameObject[] Doors;

    // Start is called before the first frame update
    void Start()
    {
        Enemyleft.Add(3);
        Enemyleft.Add(6);
        Enemyleft.Add(9);
        Enemyleft.Add(1);
 

        for (int i =0; i< stage1EnemyObj.Count; i++)
        {
            stage1EnemyObj[i].SetActive(false);
        }
        for (int i = 0; i < stage2EnemyObj.Count; i++)
        {
            stage2EnemyObj[i].SetActive(false);
        }
        for (int i = 0; i < stage3EnemyObj.Count; i++)
        {
            stage3EnemyObj[i].SetActive(false);
        }
        for (int i = 0; i < stage4EnemyObj.Count; i++)
        {
            stage4EnemyObj[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Stage1 == true)
        {
            Doors[0].GetComponent<DoorControl>().StageClear();
            for (int i = 0; i < stage2EnemyObj.Count; i++)
            {
                stage2EnemyObj[i].SetActive(true);
            }
        }
        if (Stage2 == true)
        {
            Doors[1].GetComponent<DoorControl>().StageClear();
            for (int i = 0; i < stage3EnemyObj.Count; i++)
            {
                stage3EnemyObj[i].SetActive(true);
            }

        }
        if (Stage3 == true)
        {
            Doors[2].GetComponent<DoorControl>().StageClear();
            for (int i = 0; i < stage4EnemyObj.Count; i++)
            {
                stage4EnemyObj[i].SetActive(true);
            }

        }
        if (Stage4 == true)
        {
            Doors[3].GetComponent<DoorControl>().StageClear();
        }
    }

    public void howEnemyleft(int a)
    {
        Enemyleft[a - 1] -= 1;
        
        if (Enemyleft[0] == 0)
        {
            Debug.Log("1스테이지 클리어");
            Stage1 = true;
        }
        if (Enemyleft[1] == 0)
        {
            Debug.Log("2스테이지 클리어");
            Stage2 = true;
        }
        if (Enemyleft[2] == 0)
        {
            Debug.Log("3스테이지 클리어");
            Stage3 = true;
        }
        if (Enemyleft[3] == 0)
        {
            Debug.Log("4스테이지 클리어");
            Stage4 = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < stage1EnemyObj.Count; i++)
            {
                stage1EnemyObj[i].SetActive(true);
            }
        }
    }
}
