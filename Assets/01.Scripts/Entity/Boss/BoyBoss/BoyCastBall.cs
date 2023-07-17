using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyCastBall : BossAttackProjectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player p) && _mode == 1)
        {
            p.Damage(_damage);
            p.AddEffect(new EffectStun(1, 2, MotherBoss));
            Destroy(this.gameObject);
        }
        if (collision.gameObject == MotherBoss.gameObject && _mode == 2)
        {
            MotherBoss.Damage(_hittedDamage);
            Destroy(this.gameObject);
        }
        if (_mode != 0 && !collision.gameObject == MotherBoss.gameObject && !collision.TryGetComponent(out Player pa))
            Destroy(this.gameObject);
    }
}
