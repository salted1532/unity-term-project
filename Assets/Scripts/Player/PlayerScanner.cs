using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScanner : MonoBehaviour
{
    public PlayerWeaponMgr playerWeaponMgr;
    float delayTime;
    public bool ChangeTrigger;
    public int curScanWeaponNum = -1;
    public int LastScanWeaponNum = -1;
    private List<Items> detectedWeapons = new List<Items>(); // ������ ���� ���
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && detectedWeapons.Count > 0)
        {
            // ���� ����� ���� ã��
            Items closestWeapon = GetClosestWeapon();
            if (closestWeapon != null)
            {
                // ���� ��ȣ ����
                curScanWeaponNum = closestWeapon.GetNum();
                PlayerInventory.WeaponRoting(curScanWeaponNum);
                ChangeTrigger = true;

                // �ٴ��� ���� ����
                detectedWeapons.Remove(closestWeapon);
                closestWeapon.gameObject.SetActive(false);
                LastScanWeaponNum = curScanWeaponNum;

                //test
                Debug.Log("���� ȹ��: " + curScanWeaponNum);
                int[] arr = PlayerInventory.GetWeaponArray();
                Debug.Log("�κ� ī��Ʈ: " + arr.Length);

                for (int i = 0; i < arr.Length; i++)
                {
                    Debug.Log(i + "ĭ° ���� ��ȣ: " +arr[i]);
                }
                //���� ���� ����
                playerWeaponMgr.SetCurWeaponData();
            }
        }
    }

    private Items GetClosestWeapon()
    {
        Items closestWeapon = null;
        float closestDistance = Mathf.Infinity;

        foreach (Items weapon in detectedWeapons)
        {
            float distance = Vector3.Distance(transform.position, weapon.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWeapon = weapon;
            }
        }

        return closestWeapon;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Item"))
        {
            curScanWeaponNum = other.gameObject.GetComponent<Items>().GetNum();
            Debug.Log("���� ��ȣ: " + curScanWeaponNum);
        }
        if (other.CompareTag("Item"))
        {
            Items weapon = other.GetComponent<Items>();
            if (weapon != null && !detectedWeapons.Contains(weapon) && weapon.GetNum() % 10 != 0)
            {
                detectedWeapons.Add(weapon);
            }
        }
    }
  
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Item"))
        {
            Items weapon = other.GetComponent<Items>();
            if (weapon != null)
            {
                detectedWeapons.Remove(weapon);
                if(detectedWeapons.Count == 0)
                {
                    curScanWeaponNum = -1; 
                }
            }
        }
    }
}
