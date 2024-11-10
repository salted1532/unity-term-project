using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMusicControl : MonoBehaviour
{
    public GameObject Player;
    public GameObject movepos;
    public GameObject StartFightObject;

    private StageControl stagecontrol;
    private FightMusicControl fightmusic;

    public int stage = 1;
    public bool isFightStart = false;
    public bool isStopFightGameObject = false;

    void Start()
    {
        //Player.transform.position = movepos.transform.position;

        stagecontrol = GameObject.Find("StageManage").GetComponent<StageControl>();
        if(isStopFightGameObject == true)
        {
            fightmusic = GameObject.Find("StartFight-" + stage).GetComponent<FightMusicControl>();
        }
    }

    void Update()
    {
        
    }

    public void Set_isFightStart(bool value)
    {
        isFightStart = value;
    }

    public bool Get_isFightStart()
    {
        return isFightStart;
    }

    public void StopAllSound()
    {
        if(SoundManager.Instance.IsSoundInLoop("hl2_song2") == true)
        {
            SoundManager.Instance.StopLoopSound("hl2_song2");
        }
        else if(SoundManager.Instance.IsSoundInLoop("hl1_song10") == true)
        {
            SoundManager.Instance.StopLoopSound("hl1_song10");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(stagecontrol.IsStageClear(stage) == true)
            {
               
            }
            else
            {
                if(isStopFightGameObject == true && fightmusic != null)
                {
                    if(fightmusic.Get_isFightStart() == true)
                    {
                        StopAllSound();
                        SoundManager.Instance.PlaySound2D("hl2_song2", 0f, true, SoundType.BGM);
                        fightmusic.Set_isFightStart(false);
                    }
                }
                else
                {
                    if(isFightStart == false)
                    {
                        StopAllSound();
                        SoundManager.Instance.PlaySound2D("hl1_song10", 0f, true, SoundType.BGM);
                        isFightStart = true;
                    }
                }
            }
        }
    }
}
