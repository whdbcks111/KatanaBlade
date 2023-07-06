
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
        _anim = GetComponentInChildren<Animator>();
    }

    protected override void MonsterMove()
    {
        base.MonsterMove();

        _anim.SetFloat("WalkSpeed", Mathf.Abs(_nextMove));
    }

    public override void StartAttack(Player other)
    {
        base.StartAttack(other);
        _anim.SetTrigger("Attack");
    }

    public override void Attack(Entity other)
    {
        other.Damage(10);
    }

}