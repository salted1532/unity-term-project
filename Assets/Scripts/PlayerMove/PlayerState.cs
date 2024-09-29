using System.Collections.Generic;
using UnityEngine;
public static class PlayerState
{
    public static float PlayerAttackMaxCooldownTime;
    public static Vector3 PlayerCurPos;
    public static bool PlayerIsDashing;

    static void swapWeapon(float _MaxCooldownTime)
    {
        PlayerState.PlayerAttackMaxCooldownTime = _MaxCooldownTime;
    }
}
