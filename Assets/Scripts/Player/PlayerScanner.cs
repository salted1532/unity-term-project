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
    private List<Items> detectedWeapons = new List<Items>(); // 감지된 무기 목록
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && detectedWeapons.Count > 0)
        {
            // 가장 가까운 무기 찾기
            Items closestWeapon = GetClosestWeapon();
            if (closestWeapon != null)
            {
                // 무기 번호 저장
                curScanWeaponNum = closestWeapon.GetNum();
                PlayerInventory.WeaponRoting(curScanWeaponNum);
                ChangeTrigger = true;

                // 바닥의 무기 제거
                detectedWeapons.Remove(closestWeapon);
                closestWeapon.gameObject.SetActive(false);
                LastScanWeaponNum = curScanWeaponNum;

                //test
                Debug.Log("무기 획득: " + curScanWeaponNum);
                int[] arr = PlayerInventory.GetWeaponArray();
                Debug.Log("인벤 카운트: " + arr.Length);

                for (int i = 0; i < arr.Length; i++)
                {
                    Debug.Log(i + "칸째 무기 번호: " +arr[i]);
                }
                //현재 무기 세팅
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
            Debug.Log("감지 번호: " + curScanWeaponNum);
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
