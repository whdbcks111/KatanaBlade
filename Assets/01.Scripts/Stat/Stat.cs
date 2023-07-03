using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    private readonly Dictionary<StatType, float> _defaultValues = new(), _addValues = new(), _multiplyValues = new();

    public Stat()
    {
        StatType[] statTypes = (StatType[]) System.Enum.GetValues(typeof(StatType));
        foreach(var type in statTypes)
        {
            _defaultValues[type] = 0f;
        }
    }
}

public enum StatType
{
    MaxHP, // 최대 HP
    MaxDashStamina, // 대시 게이지 최대치
    MaxParryingStamina, // 패링 게이지 최대치
    ParryingSpeed, // 패링 속도
    ParryingAttackForce, // 패링 공격력 
    JumpForce, // 점프력
    MoveSpeed, // 이동 속도
    DashCooldown, // 대시 쿨타임
    ParryingStaminaRegen, // 스태미나 재생력
    DashStaminaRegen // 대시 재생력
}
