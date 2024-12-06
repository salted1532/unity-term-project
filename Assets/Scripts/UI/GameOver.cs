using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

public class GameOver : MonoBehaviour
{
    public GameObject BloodImage;
    public GameObject MainMenuButton;
    public GameObject BlackScreen;
    public GameObject Player;
    public GameObject GameOverText;
    public GameObject Inventory;

    public CanvasGroup canvasGroup;
    public TextMeshPro textMeshPro;

    private float time = 0f;
    private float BloodAlpha = 1f;
    private float ButtonAlpha = 0f;

    private float gameover_speed = 0.025f;
    private float popup_delay = 0.5f;

    private Color originalColor;

    private bool isGameOver = false;
    private bool ShowButton = false;

    void Start()
    {
        gameObject.SetActive(false);
        
        SetCanvasInteractable(false);
        SetCanvasAlpha(ButtonAlpha);

        originalColor = textMeshPro.color;
        textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    void Update()
    {
        if(isGameOver == true)
        {
            time += Time.deltaTime;
            if(time>gameover_speed)
            {
                time = time - gameover_speed;
                BloodAlpha = BloodAlpha - gameover_speed;
                BloodImage.GetComponent<Image>().color = new Color(1, 1, 1, BloodAlpha);
                if(BloodAlpha <= 0)
                {
                    isGameOver = false;
                    Invoke("ShowMainMenuButton",popup_delay);
                }
            }
        }
        if(ShowButton == true)
        {
            time = time + Time.deltaTime;
            if(time>gameover_speed)
            {
                time = time - gameover_speed;
                ButtonAlpha = ButtonAlpha + gameover_speed;
                textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, ButtonAlpha);

                SetCanvasAlpha(ButtonAlpha);
                if(ButtonAlpha >= 1f)
                {
                    ShowButton = false;
                    SetCanvasInteractable(true);
                    GameOverText.GetComponent<BlickTitle>().StartBlink();

                    Time.timeScale = 1;
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
        Inventory.SetActive(false);
        Player.GetComponent<PlayerController>().DisablePlayer();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Invoke("HideBloodImage",popup_delay);
    }
}
