using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : Entity
{
    public Boss MotherBoss;
    [SerializeField] private int _damage;
    [SerializeField] private bool _parryAble;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player p))
        {
            p.Damage(_damage);
        }
    }
    public override void Damage(float damage)
    {
        if (_parryAble)
            MotherBoss.Damage(damage);
    }

}
