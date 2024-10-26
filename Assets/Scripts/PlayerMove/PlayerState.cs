using System.Collections.Generic;
using UnityEngine;
public static class PlayerState
{
    public static float PlayerAttackMaxCooldownTime;
    public static float PlayerDashMaxCooldownTime =1f;
    public static Vector3 PlayerCurPos;
    public static bool PlayerIsDashing;


    static void SetMaxCooldownTIme(float _MaxCooldownTime)
    {
        PlayerState.PlayerAttackMaxCooldownTime = _MaxCooldownTime;
    }


}
