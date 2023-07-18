using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackProjectile : Entity
{
    private Player _player;
    [SerializeField] protected int _mode;
    public Boss MotherBoss;
    [SerializeField] protected int _damage;
    protected Vector2 _targetSpot;
    protected float _hittedDamage;
    [SerializeField] protected float _speed;
    protected virtual void Start()
    {
        _player = FindObjectOfType<Player>();
        setTarget(_player.transform.position);
    }
    protected override void FixedUpdate()
    {
        if (_mode == 0)
            setTarget(_player.transform.position);
        else
            Move();
    }
    public void Fire()
    {
        _mode = 1;
        Destroy(this.gameObject, 12.0f);
    }

    public void setTarget(Vector2 pos)
    {
        _targetSpot = pos;
        float angle = ExtraMath.DirectionToAngle((Vector3)_targetSpot - transform.position);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void Move()
    {
        transform.Translate(Vector2.right * Time.deltaTime * _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player p) && _mode == 1)
        {
            p.Damage(_damage);
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
    public override void Damage(float damage)
    {
        _hittedDamage = _damage + damage;
        _mode = 2;
        _speed *= 2;
        setTarget(MotherBoss.transform.position);
    }
}
