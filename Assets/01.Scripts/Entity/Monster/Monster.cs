using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
        Debug.Log("Gold Drop");
        int GoldDrop = 5;
        float spread = 0.8f;
        {
            GameObject load = Resources.Load<GameObject>("Item/Gold");
            while (GoldDrop > 0)
            {
                GameObject gold = Instantiate(load, transform.position, Quaternion.identity);
                GoldDrop--;
                gold.transform.position = new Vector2(spread * Random.value - spread / 2,
                    spread * Random.value - spread / 2);

                //GoldDrop -= 1;
                //Vector3 position = transform.position;
                //position.x += spread * UnityEngine.Random.value - spread / 2;
                //position.y += spread * UnityEngine.Random.value - spread / 2;
            }
        }
    }
}
