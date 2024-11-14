using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPauseUI : MonoBehaviour
{
    public bool isPause = false;

    public GameObject MainMenuButton;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void ChangePauseState()
    {
        isPause = !isPause;

        if(isPause == true)
        {
            MainMenuButton.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            MainMenuButton.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; 
            Time.timeScale = 1;
        }
    }
}
