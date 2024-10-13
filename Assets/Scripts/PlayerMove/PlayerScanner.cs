using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScanner : MonoBehaviour
{
    public bool ChangeTrigger;
    public int curScanWeaponNum = -1;
    public int ScanWeaponNum = -1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            curScanWeaponNum = other.gameObject.GetComponent<Items>().GetNum();

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
            Debug.Log("°¨ÁöÁß!");

        if (other.CompareTag("Item") && Input.GetKeyDown(KeyCode.E))
        {
            ScanWeaponNum = curScanWeaponNum;

            other.gameObject.SetActive(false);
            PlayerInventory.WeaponRoting(ScanWeaponNum);
            ChangeTrigger = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            curScanWeaponNum = -1;
        }
    }
}
