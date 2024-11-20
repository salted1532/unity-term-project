using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPauseUI : MonoBehaviour
{
    public bool isPause = false;
    public GameObject Player;

    void Start()
    {
        gameObject.SetActive(true);
        Debug.Log(isPause);
    }

    void Update()
    {
        
    }

    public void ChangePauseState()
    {
        isPause = !isPause;

        if(isPause == true)
        {
            gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Player.GetComponent<PlayerController>().SetController(false);

            Time.timeScale = 0;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; 
            Player.GetComponent<PlayerController>().SetController(true);

            Time.timeScale = 1;
        }
    }
}
