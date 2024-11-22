using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDataManager : MonoBehaviour
{
    public static float MASTER = 0.5f;
    public static float BGM = 0.5f;
    public static float EFFECT = 0.5f;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        
    }

    public void SetSoundValue(SoundType type,float value)
    {
        switch(type.ToString())
        {
            case "MASTER":
                MASTER = value;
                break;
            case "BGM":
                BGM = value;
                break;
            case "EFFECT":
                EFFECT = value;
                break;
            default:
                Debug.Log("SOUND VALUE ERROR");
                break;
        }
    }

    public float GetMasterValue()
    {
        return MASTER;
    }

    public float GetBGMValue()
    {
        return BGM;
    }

    public float GetEffectValue()
    {
        return EFFECT;
    }
}
