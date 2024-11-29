using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject BloodImage;
    public GameObject MainMenuButton;
    public GameObject BlackScreen;
    public GameObject Player;
    

    public CanvasGroup canvasGroup;

    private float time = 0f;
    private float BloodAlpha = 1f;
    private float ButtonAlpha = 0f;

    private bool isGameOver = false;
    private bool ShowButton = false;

    void Start()
    {
        gameObject.SetActive(false);
        
        SetCanvasInteractable(false);
        SetCanvasAlpha(ButtonAlpha);
    }

    void Update()
    {
        if(isGameOver == true)
        {
            time += Time.deltaTime;
            if(time>0.01f)
            {
                time = time - 0.01f;
                BloodAlpha = BloodAlpha - 0.01f;
                BloodImage.GetComponent<Image>().color = new Color(1, 1, 1, BloodAlpha);
                if(BloodAlpha <= 0)
                {
                    isGameOver = false;
                    Invoke("ShowMainMenuButton",0.5f);
                }
            }
        }
        if(ShowButton == true)
        {
            time = time + Time.deltaTime;
            if(time>0.01f)
            {
                time = time - 0.01f;
                ButtonAlpha = ButtonAlpha + 0.01f;
                SetCanvasAlpha(ButtonAlpha);
                if(ButtonAlpha >= 1f)
                {
                    ShowButton = false;
                    SetCanvasInteractable(true);
                    Time.timeScale = 0;
                }
            }
        }
    }

    public void SetCanvasAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
    }

    public void SetCanvasInteractable(bool interactable)
    {
        canvasGroup.interactable = interactable;
    }

    public void ShowMainMenuButton()
    {
        ShowButton = true;
    }

    public void HideBloodImage()
    {
        isGameOver = true;
    }

    public void LoadMainMenuScene()
    {
        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Main Menu");
    }

    public void GameOverEvent()
    {
        gameObject.SetActive(true);
        Player.GetComponent<PlayerController>().DisablePlayer();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Invoke("HideBloodImage",0.5f);
    }
}
