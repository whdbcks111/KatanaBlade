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

    private Rigidbody2D _rigid;

    public float MovingVelocity = 0, KnockbackVelocity = 0;

    public StatusEffect[] Effects { get { return _effects.ToArray(); } }

    public void Init()
    {
        HP = Stat.Get(StatType.MaxHP);
        DashStamina = Stat.Get(StatType.MaxDashStamina);
        ParryingStamina = Stat.Get(StatType.MaxParryingStamina);
    }
    protected virtual void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        Init();
    }

    public void Teleport(Vector3 pos)
    {
        transform.position = pos;
        if (Camera.main.TryGetComponent(out CameraControll cc)) cc.Teleport();
    }

    public void LateAct(Action action)
    {
        _lateActions.Enqueue(action);
    }

    protected virtual void Update()
    {
        UpdateEffects();

        if (Input.GetKeyDown(KeyCode.K)) Knockback(30);
    }

    protected virtual void FixedUpdate()
    {
        VelocityUpdate();
    }

    private void VelocityUpdate()
    {
        var vel = _rigid.velocity;
        vel.x = MovingVelocity + KnockbackVelocity;
        _rigid.velocity = vel;
        KnockbackVelocity = Mathf.MoveTowards(KnockbackVelocity, 0f, Time.deltaTime * 30);
    }

    public void Knockback(float power)
    {
        KnockbackVelocity += power;
    }

    public void AddEffect(StatusEffect eff)
    {
        _effects.Add(eff);
    }

    public bool HasEffect<T>() where T : StatusEffect
    {
        return _effects.Find(e => e is T) is not null;
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
