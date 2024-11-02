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
        Doors[0].GetComponent<DoorControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Stage1 == true)
        {
            Doors[0].GetComponent<DoorControl>().StageClear();
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
    }
}
