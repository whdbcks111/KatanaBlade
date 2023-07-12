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
        Debug.Log("코인 드랍함");
        
    }
    protected bool _isStun;


    protected override void Awake()
    {
        base.Awake();
        this.Stat.SetDefault(StatType.MaxHP, 80);
        LateAct(() =>
        {
            this.HP = this.MaxHP;
        });
    }

}
