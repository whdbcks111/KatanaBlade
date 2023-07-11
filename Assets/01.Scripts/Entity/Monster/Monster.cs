using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Entity
{
    protected int GoldDrop;
    public override void Damage(float damage)
    {
        base.Damage(damage);
        if(HP <= 0)
            OnMonsterDie();
    }

    public virtual void OnMonsterDie()
    {
        Debug.Log("���� �����");

    }
    protected bool _isStun;
}
