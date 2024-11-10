using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl1 : MonoBehaviour
{

    public List<int> Enemyleft = new List<int>();
    public List<GameObject> stage1EnemyObj = new List<GameObject>();
    public List<GameObject> stage2EnemyObj = new List<GameObject>();
    public List<GameObject> stage3EnemyObj = new List<GameObject>();
    public List<GameObject> stage4EnemyObj = new List<GameObject>();

    public bool Stage1 = false, Stage2 = false, Stage3 = false, Stage4 = false;

    public GameObject[] Doors;
    private FightMusicControl fightmusic;

    // Start is called before the first frame update
    void Start()
    {
        Enemyleft.Add(3);
        Enemyleft.Add(6);
        Enemyleft.Add(9);
        Enemyleft.Add(1);
        Doors[0].GetComponent<DoorControl>();
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
        fightmusic = GameObject.Find("FightMusic").GetComponent<FightMusicControl>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartNormalMusic()
    {
        fightmusic.StopAllSound();
        SoundManager.Instance.PlaySound2D("hl2_song2", 0f, true, SoundType.BGM);
    }

    void SetNextStage(int stage)
    {
        if (stage == 1)
        {
            Doors[0].GetComponent<DoorControl>().StageClear();
            for (int i = 0; i < stage2EnemyObj.Count; i++)
            {
                stage2EnemyObj[i].SetActive(true);
            }
            StartNormalMusic();
        }
        if (stage == 2)
        {
            Doors[1].GetComponent<DoorControl>().StageClear();
            for (int i = 0; i < stage3EnemyObj.Count; i++)
            {
                stage3EnemyObj[i].SetActive(true);
            }
            StartNormalMusic();
        }
        if (stage == 3)
        {
            Doors[2].GetComponent<DoorControl>().StageClear();
            for (int i = 0; i < stage4EnemyObj.Count; i++)
            {
                stage4EnemyObj[i].SetActive(true);
            }
            StartNormalMusic();
        }   
        if (stage == 4)
        {
            Doors[3].GetComponent<DoorControl>().StageClear();
            StartNormalMusic();
        }
    }

    public void howEnemyleft(int a)
    {
        Enemyleft[a - 1] -= 1;

        if (Enemyleft[0] == 0 && Stage1 == false)
        {
            Debug.Log("1�������� Ŭ����");
            Stage1 = true;
            SetNextStage(1);
        }
        if (Enemyleft[1] == 0 && Stage2 == false)
        {
            Debug.Log("2�������� Ŭ����");
            Stage2 = true;
            SetNextStage(2);
        }
        if (Enemyleft[2] == 0 && Stage3 == false)
        {
            Debug.Log("3�������� Ŭ����");
            Stage3 = true;
            SetNextStage(3);
        }
        if (Enemyleft[3] == 0 && Stage4 == false)
        {
            Debug.Log("4�������� Ŭ����");
            Stage4 = true;
            SetNextStage(4);
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
