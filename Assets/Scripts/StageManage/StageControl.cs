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
    private FightMusicControl fightmusic;

    public bool GameStart = false;

    // Start is called before the first frame update
    void Start()
    {
        Enemyleft.Add(3);
        Enemyleft.Add(6);
        Enemyleft.Add(9);
        Enemyleft.Add(1);

        fightmusic = GameObject.Find("FightMusic").GetComponent<FightMusicControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Stage1 == true)
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

        if(GameStart == false)
        {
            for (int i = 0; i < stage1EnemyObj.Count; i++)
            {
                stage1EnemyObj[i].SetActive(false);
            }
        }
        if (Stage1 == false) 
        {
            for (int i = 0; i < stage2EnemyObj.Count; i++)
            {
                stage2EnemyObj[i].SetActive(false);
            }
        }
        if (Stage2 == false)
        {
            for (int i = 0; i < stage3EnemyObj.Count; i++)
            {
                stage3EnemyObj[i].SetActive(false);
            }
        }
        if (Stage3 == false)
        {
            for (int i = 0; i < stage4EnemyObj.Count; i++)
            {
                stage4EnemyObj[i].SetActive(false);
            }

        }

    }

    void StartNormalMusic()
    {
        fightmusic.StopAllSound();
        SoundManager.Instance.PlaySound2D("hl2_song2", 0f, true, SoundType.BGM);
    }

    public void howEnemyleft(int a)
    {
        Enemyleft[a - 1] -= 1;
        
        if (Enemyleft[0] == 0)
        {
            Debug.Log("1�������� Ŭ����");
            if (Stage1 == false)
            {
                for (int i = 0; i < stage2EnemyObj.Count; i++)
                {
                    stage2EnemyObj[i].SetActive(true);
                }
            }
            Stage1 = true;

            StartNormalMusic();
        }
        if (Enemyleft[1] == 0)
        {
            Debug.Log("2�������� Ŭ����");
            if (Stage2 == false)
            {
                for (int i = 0; i < stage3EnemyObj.Count; i++)
                {
                    stage3EnemyObj[i].SetActive(true);
                }
            }
            Stage2 = true;


            StartNormalMusic();
        }
        if (Enemyleft[2] == 0)
        {
            Debug.Log("3�������� Ŭ����");
            if (Stage3 == false)
            {
                for (int i = 0; i < stage4EnemyObj.Count; i++)
                {
                    stage4EnemyObj[i].SetActive(true);
                }
            }
            Stage3 = true;

            StartNormalMusic();
        }
        if (Enemyleft[3] == 0)
        {
            Debug.Log("4�������� Ŭ����");
            Stage4 = true;
            StartNormalMusic();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < stage1EnemyObj.Count; i++)
            {
                stage1EnemyObj[i].SetActive(true);
                GameStart = true;
            }
        }
    }

    public bool IsStageClear(int stage)
    {
        if(Enemyleft[stage-1] == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
