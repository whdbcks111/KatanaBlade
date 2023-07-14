using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMonster : MeleeMonster
{
    Animator _anim;

    protected override void Awake()
    {
        base.Awake();
        _anim = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        HP -= Time.deltaTime * 10f;

        _attackSpeed = HP < MaxHP / 2 ? 1.3f : 1;
        _anim.SetBool("IsAttacking", _isAttacking);
        _anim.SetFloat("AttackSpeed", _attackSpeed);
    }

    protected override void MonsterMove()
    {
        base.MonsterMove();

        print(Mathf.Abs(MovingVelocity));
        _anim.SetFloat("WalkSpeed", Mathf.Abs(MovingVelocity));
    }

    public override void Attack(Entity other)
    {
        other.Damage(10);
    }

    public override void Damage(float damage)
    {
        HP -= damage;
        _anim.SetTrigger("Hit");
        base.Damage(damage);

        if (HP <= 0)
        {

            _anim.SetBool("Dead", true);
            StopAllCoroutines();

            Destroy(gameObject);

        }
    }

    public override void OnMonsterDie()
    {
        throw new NotImplementedException();
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
       
    }
}

