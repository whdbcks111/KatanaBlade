
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkullMonster : MeleeMonster
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

        _anim.SetBool("IsAttacking", _isAttacking);

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

    public override void Heal(float amount)
    {
        base.Heal(amount);
    }
}