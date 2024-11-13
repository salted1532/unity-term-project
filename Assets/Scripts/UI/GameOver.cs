using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    public GameObject BloodImage;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void HideBloodImage()
    {

    }

    public void GameOverEvent()
    {
        gameObject.SetActive(true);
        
    }
}
