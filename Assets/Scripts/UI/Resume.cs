using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    public GameObject Player;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ChangePause()
    {
        Player.GetComponent<PlayerController>().ChangePause();
    }
}
