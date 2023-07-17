using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stat
{
    private readonly Dictionary<StatType, float>
        _defaultValues = new()
        {
            [StatType.MaxHP] = 100f,
            [StatType.MaxParryingStamina] = 100f,
            [StatType.MaxDashStamina] = 100f,
            [StatType.ParryingTime] = 0.2f,
            [StatType.ParryingStaminaRegen] = 10f,
            [StatType.DashCooldown] = 2f,
            [StatType.DashStaminaRegen] = 10f,
            [StatType.JumpForce] = 24f,
            [StatType.ParryingAttackForce] = 10f,
            [StatType.MoveSpeed] = 12f,
            [StatType.DashLength] = 10f,
            [StatType.DashCost] = 30f,
            [StatType.ParryingCost] = 4f,
            [StatType.LowParryingFeedback] = 20f,
            [StatType.MiddleParryingFeedback] = 2f,
            [StatType.HighParryingFeedback] = 3f,
            [StatType.GoldObtainMultiplier] = 1f,
            [StatType.EssenceCooldown] = 1f,
            [StatType.EssenceForce] = 1f,
            [StatType.BossAttackForce] = 10f,
        }, 
        _addValues = new(), 
        _multiplyValues = new(), 
        _currentValues = new();

    public Stat()
    {
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

    public void SetDefault(StatType type, float defaultVal)
    {
        _defaultValues[type] = defaultVal;
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
    GoldObtainMultiplier, // 코인 획득 배율
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
    EssenceCooldown, //에센스 쿨타임 배율
    EssenceForce, //에센스 효과 배율
    BossAttackForce, //보스 대미지 배율
}
