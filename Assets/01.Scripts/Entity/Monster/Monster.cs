using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Monster : Entity
{
    protected int GoldDrop;

    protected override void Awake()
    {
        base.Awake();
        this.Stat.SetDefault(StatType.MaxHP, 80);
        LateAct(() =>
        {
            this.HP = this.MaxHP;
        });
    }

    public override void Damage(float damage)
    {
        base.Damage(damage + Player.Instance.Stat.Get(StatType.ParryingAttackForce));
        if(HP <= 0)
            OnMonsterDie();
    }

    public virtual void OnMonsterDie()
    {
        Debug.Log("Gold Drop");
        int GoldDrop = 5 + Random.Range(-2, 3);
        {
            GameObject load = Resources.Load<GameObject>("Interactable/Gold");
            while (GoldDrop > 0)
            {
                Instantiate(load, transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)), Quaternion.identity);
                GoldDrop--;

                //GoldDrop -= 1;
                //Vector3 position = transform.position;
                //position.x += spread * UnityEngine.Random.value - spread / 2;
                //position.y += spread * UnityEngine.Random.value - spread / 2;
            }
        }

        Destroy(gameObject);
    }

}
