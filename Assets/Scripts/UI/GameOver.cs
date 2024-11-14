using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    public GameObject BloodImage;
    public GameObject MainMenuButton;

    private float time = 0f;
    private float a = 1f;
    private bool isGameOver = false;
    private bool ShowButton = false;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(isGameOver == true)
        {
            time += Time.deltaTime;
            if(time>0.01f)
            {
                time = 0f;
                a = a - 0.01f;
                BloodImage.GetComponent<Image>().color = new Color(1, 1, 1, a);
                if(a <= 0)
                {
                    isGameOver = false;
                    Invoke("ShowMainMenuButton",1);
                }
            }
        }
        if(ShowButton == true)
        {
            time += Time.deltaTime;
            if(time>0.1f)
            {
                time = 0f;
                a = a + 0.1f;
                MainMenuButton.GetComponent<Image>().color = new Color(1, 1, 1, a);
                if(a >= 1)
                {
                    Time.timeScale = 0;
                }
            }
        }
    }

    public void ShowMainMenuButton()
    {
        ShowButton = true;
    }

    public void HideBloodImage()
    {
        isGameOver = true;
    }

    public void GameOverEvent()
    {
        gameObject.SetActive(true);
        Invoke("HideBloodImage",1);
    }
}
