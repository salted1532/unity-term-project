using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInventory 
{
    static int choiceIndex;
    
    static int maxWeaponCount = 3;
    static List<int> inventory = new List<int>();

    
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
    //현재 무기 int 리턴
    public static int GetCurWeaponNum() { return inventory[choiceIndex]; }

    //선택 중인 무기 기준 정렬된 무기 int 배열 리턴
    public static int[] GetWeaponArray()
    {
        if(inventory.Count == 0 || inventory.Count > maxWeaponCount)
        {
            return null;
        }
        int[] arr = new int[inventory.Count];

        for(int i = 0; i < inventory.Count - 1; ++i)
        {
            int curIndex = (choiceIndex + i < inventory.Count -1) ? choiceIndex + i: choiceIndex + i - inventory.Count;
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
        }
        else if (inventory.Count >= maxWeaponCount)
        {
            inventory[choiceIndex] = WeaponNum;
        }
    }

    public static void SwapWeapon(float MouseScrollWheel)
    {

            if (MouseScrollWheel > 0 && !(choiceIndex == maxWeaponCount - 1))
            {
                // 휠을 밀어 돌렸을 때의 처리 ↑
                ++choiceIndex;
            }

            else if (MouseScrollWheel < 0 && !(choiceIndex == 0))
            {
                // 휠을 당겨 올렸을 때의 처리 ↓
                --choiceIndex;

            }
    }
}
