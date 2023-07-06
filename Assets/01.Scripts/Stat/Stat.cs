using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stat
{
    private readonly Dictionary<StatType, float> 
        _defaultValues = new(), 
        _addValues = new(), 
        _multiplyValues = new(), 
        _currentValues = new();

    public Stat()
    {
        _defaultValues[StatType.MaxHP] = 100f;
        _defaultValues[StatType.MaxParryingStamina] = 100f;
        _defaultValues[StatType.MaxDashStamina] = 100f;
        _defaultValues[StatType.ParryingTime] = 0.2f;
        _defaultValues[StatType.ParryingStaminaRegen] = 10f;
        _defaultValues[StatType.DashCooldown] = 2f;
        _defaultValues[StatType.DashStaminaRegen] = 10f;
        _defaultValues[StatType.JumpForce] = 9f;
        _defaultValues[StatType.ParryingAttackForce] = 10f;
        _defaultValues[StatType.MoveSpeed] = 5f;
        _defaultValues[StatType.DashLength] = 4f;
        _defaultValues[StatType.DashCost] = 30f;
        _defaultValues[StatType.ParryingCost] = 30f;
        _defaultValues[StatType.LowParryingFeedback] = 1f;
        _defaultValues[StatType.MiddleParryingFeedback] = 2f;
        _defaultValues[StatType.HighParryingFeedback] = 3f;

        InitStats();
        UpdateStatValues();
    }

    private void InitStats()
    {
        StatType[] statTypes = (StatType[])System.Enum.GetValues(typeof(StatType));
        foreach (var type in statTypes)
        {
            _addValues[type] = 0f;
            _multiplyValues[type] = 1f;
        }
    }

    private void UpdateStatValues()
    {
        StatType[] statTypes = (StatType[])System.Enum.GetValues(typeof(StatType));
        foreach (var type in statTypes)
        {
            _currentValues[type] = (_defaultValues[type] + _addValues[type]) * _multiplyValues[type];
        }
    }

    public void Update()
    {
        UpdateStatValues();
        InitStats();
    }

    public void Add(StatType type, float addition)
    {
        _addValues[type] += addition;
    }

    public void Multiply(StatType type, float multiplier)
    {
        _multiplyValues[type] *= multiplier;
    }

    public float Get(StatType type)
    {
        return _currentValues[type];
    }
}

public enum StatType
{
    MaxHP, // 최대 HP
    MaxDashStamina, // 대시 게이지 최대치
    MaxParryingStamina, // 패링 게이지 최대치
    ParryingTime, // 패링 속도
    ParryingAttackForce, // 패링 공격력 
    ParryingCost, // 패링 비용
    JumpForce, // 점프력
    MoveSpeed, // 이동 속도
    DashCooldown, // 대시 쿨타임
    DashCost , // 대시 비용
    ParryingStaminaRegen, // 스태미나 재생력
    DashStaminaRegen, // 대시 재생력
    DashLength, // 대시 거리
    LowParryingFeedback, // 패링 피드백 (약)
    MiddleParryingFeedback, // 패링 피드백 (중)
    HighParryingFeedback, // 패링 피드백 (강)
}
