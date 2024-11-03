using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : MonoBehaviour
{

    public List<int> Enemyleft = new List<int>();

    public bool Stage1 = false, Stage2 = false, Stage3 = false, Stage4 = false;

    public GameObject[] Doors;

    // Start is called before the first frame update
    void Start()
    {
        Enemyleft.Add(3);
        Enemyleft.Add(0);
        Enemyleft.Add(0);
        Enemyleft.Add(0);
        Doors[0].GetComponent<DoorControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Stage1 == true)
        {
            Doors[0].GetComponent<DoorControl>().StageClear();
        }
        if (Stage2 == true)
        {
            Doors[1].GetComponent<DoorControl>().StageClear();
        }
        if (Stage3 == true)
        {
            Doors[2].GetComponent<DoorControl>().StageClear();
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
            Debug.Log("1�������� Ŭ����");
            Stage1 = true;
        }
        if (Enemyleft[1] == 0)
        {
            Debug.Log("2�������� Ŭ����");
            Stage2 = true;
        }
        if (Enemyleft[2] == 0)
        {
            Debug.Log("3�������� Ŭ����");
            Stage3 = true;
        }
        if (Enemyleft[3] == 0)
        {
            Debug.Log("4�������� Ŭ����");
            Stage4 = true;
        }
    }
}
