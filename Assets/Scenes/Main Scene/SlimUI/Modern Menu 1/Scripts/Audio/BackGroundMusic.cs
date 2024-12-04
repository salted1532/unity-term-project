using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    void Start()
    {
        Invoke("StartBackGroundMusic",1);
    }

    void StartBackGroundMusic()
    {
        SoundManager.Instance.PlaySound2D("hl2_song2", 0f, true, SoundType.BGM);
    }

    void Update()
    {
        
    }
}
