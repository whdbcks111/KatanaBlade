using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Entity : MonoBehaviour
{
    public readonly Stat Stat = new();

    private float _hp, _dashStamina, _parryingStamina;

    public float HP { get { return _hp; } set { _hp = Mathf.Clamp(value, 0f, MaxHP); } }
    public float DashStamina { get { return _dashStamina; } set { _dashStamina = Mathf.Clamp(value, 0f, MaxDashStamina); } }
    public float ParryingStamina { get { return _parryingStamina; } set { _parryingStamina = Mathf.Clamp(value, 0f, MaxParryingStamina); } }
    public float MaxHP { get { return Stat.Get(StatType.MaxHP); } }
    public float MaxDashStamina { get { return Stat.Get(StatType.MaxDashStamina); } }
    public float MaxParryingStamina { get { return Stat.Get(StatType.MaxParryingStamina); } }

    private readonly Queue<Action> _lateActions = new();
    private readonly List<StatusEffect> _effects = new();

    private readonly HashSet<StatusEffect> _deleteEffects = new();

    public void Init()
    {
        HP = Stat.Get(StatType.MaxHP);
        DashStamina = Stat.Get(StatType.MaxDashStamina);
        ParryingStamina = Stat.Get(StatType.MaxParryingStamina);
    }

    public void LateAct(Action action)
    {
        _lateActions.Enqueue(action);
    }

    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Update()
    {
        UpdateEffects();
    }

    public void AddEffect(StatusEffect eff)
    {
        _effects.Add(eff);
    }

    private void UpdateEffects()
    {
        _deleteEffects.Clear();
        foreach(var eff in _effects)
        {
            eff.OnUpdate(this);
            
            if((eff.Duration -= Time.deltaTime) <= 0f)
            {
                eff.OnFinish(this);
                _deleteEffects.Add(eff);
            }
        }
        
        _effects.RemoveAll(eff => _deleteEffects.Contains(eff));
    }

    private void LateUpdate()
    {
        Stat.Update();
        while(_lateActions.TryDequeue(out Action action))
        {
            action();
        }
    }

    public virtual void Attack(Entity other)
    {
        var damage = 1; // 예시 값, 실제로는 계산
        other.Damage(damage);
    }

    public virtual void Damage(float damage)
    {
        // HP 닳는 코드 구현
        HP -= damage;
    }

    public virtual void Heal(float amount)
    {
        HP += amount;
    }
}
