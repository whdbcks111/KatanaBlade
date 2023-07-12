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
        var goldPrefab = Resources.Load("Interactable/Gold");
        Debug.Log("코인 드랍함");
        for(int i = 0; i < 3; ++i)
        {
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
    protected bool _isStun;
}
