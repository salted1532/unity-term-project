using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorControl : MonoBehaviour
{
    public GameObject Player;
    public GameObject movepos;

    private bool istpon = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(moveplayerpos), 0.1f);
        }
    }

    public void moveplayerpos()
    {
        Player.transform.position = new Vector3(227, 26, -105);
    }
}
