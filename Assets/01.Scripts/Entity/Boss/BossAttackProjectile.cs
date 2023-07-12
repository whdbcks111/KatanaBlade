using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackProjectile : Entity
{
    private Player _player;
    [SerializeField] private int _mode;
    public Boss MotherBoss;
    [SerializeField] private int _damage;
    private Vector2 _targetSpot;
    private int _hittedDamage;
    [SerializeField] private float _speed;
    private void Start()
    {
        _player = FindObjectOfType<Player>();
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
    }

    public void setTarget(Vector2 pos)
    {
        _targetSpot = pos;
        transform.LookAt(pos);
    }
    private void Move()
    {
        transform.position = Vector2.Lerp(transform.position, _targetSpot, _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player p))
        {
            _mode = 0;
            p.Damage(_damage);
        }
        if (collision.gameObject == MotherBoss.gameObject)
        {
            _mode = 0;
            MotherBoss.Damage(_hittedDamage);
        }
    }
    public override void Damage(float damage)
    {
        setTarget(MotherBoss.transform.position);
    }
}
