using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyCastBall : BossAttackProjectile
{
    private int ran;
    private SpriteRenderer _renderer;
    protected override void Start()
    {
        base.Start();
        ran = Mathf.FloorToInt(Random.Range(0, 7));
        _renderer = GetComponent<SpriteRenderer>();
        switch (ran)
        {
            case 0:
            case 1:
            case 2:
                _renderer.color = Color.red;
                break;
            case 3:
            case 4:
            case 5:
                _renderer.color = Color.yellow;
                break;
            case 6:
                _renderer.color = Color.green;
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player p) && _mode == 1)
        {
            switch (ran)
            {
                case 0:
                case 1:
                case 2:
                    p.AddEffect(new EffectFire(1, 2, MotherBoss));
                    break;
                case 3:
                case 4:
                case 5:
                    p.AddEffect(new EffectStun(1, 2, MotherBoss));
                    break;
                case 6:
                    p.AddEffect(new EffectRegeneration(5, 5.5f, MotherBoss));
                    break;
            }
            if (ran != 6)
                p.Damage(_damage);
            else
                p.Damage(_damage / 2);
            Destroy(this.gameObject);
        }
        if (collision.gameObject == MotherBoss.gameObject && _mode == 2)
        {
            if (ran != 6)
                MotherBoss.Damage(_hittedDamage);
            else
                MotherBoss.HP += _damage;
                Destroy(this.gameObject);
        }
        if (_mode != 0 && !collision.gameObject == MotherBoss.gameObject && !collision.TryGetComponent(out Player pa))
            Destroy(this.gameObject);
    }
}
