using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInventory 
{

    static int choiceIndex;
    
    static int maxWeaponCount = 4;
    static List<int> inventory = new List<int>() { PlayerWeaponNum.DEFULAT };

    public static void ReSetInventory()
    {
        choiceIndex = 0;
        inventory = new List<int>() { PlayerWeaponNum.DEFULAT};
    }
    public static int GetCurWeaponCount()
    {
        return inventory.Count;
    }
    public static int GetMaxWeaponCount()
    {
        return maxWeaponCount;
    }
    public static int GetChoiceIndex()
    {
        return choiceIndex;
    }
    //���� ���� int ����
    public static int GetCurWeaponNum()
    { return inventory[choiceIndex];}

    //���� ���� ���� ���� ���ĵ� ���� int �迭 ����
    public static int[] GetWeaponArray()
    {
        if(inventory.Count == 0 || inventory.Count > maxWeaponCount)
        {
            return null;
        }
        int[] arr = new int[inventory.Count];

        for (int i = 0; i < inventory.Count; ++i)
        {
            int curIndex = (choiceIndex + i) % inventory.Count;

            arr[i] = inventory[curIndex];
        }
        return arr;
    }
   public static void AddWeaponArray(int WeaponNum)
    {
        inventory.Add(WeaponNum);
    }
    public static void SetWeapon(int WeaponNum)
    {
        inventory[choiceIndex] = WeaponNum;
    }

    public static void WeaponRoting(int WeaponNum)
    {
        if (inventory.Count < maxWeaponCount && inventory.Count > 0)
        {
            inventory.Add(WeaponNum);
            choiceIndex++;
        }
        else if (inventory.Count >= maxWeaponCount)
        {
            inventory[choiceIndex] = WeaponNum;
        }
    }

    public static bool SwapWeapon(float MouseScrollWheel)
    {
        
        if (MouseScrollWheel > 0 /*&& (choiceIndex < inventory.Count -1)*/)
        {
            // ���� �о� ������ ���� ó�� ��
            choiceIndex = (choiceIndex + 1) % inventory.Count;
            Debug.Log("������ ���� �ε���:" + choiceIndex);
            return true;
        }

        else if (MouseScrollWheel < 0 /*&& !(choiceIndex == 0)*/)
        {
            // ���� ��� �÷��� ���� ó�� ��
            choiceIndex = ((choiceIndex - 1) < 0) ? (choiceIndex + inventory.Count -1): --choiceIndex;
            Debug.Log("������ ���� �ε���:" + choiceIndex);
            return true;

        }
        return false;
    }
}
