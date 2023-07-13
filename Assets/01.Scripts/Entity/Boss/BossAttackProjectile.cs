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
        Destroy(this.gameObject, 2.0f);
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
        }
        if (collision.gameObject == MotherBoss.gameObject && _mode == 2)
        {
            MotherBoss.Damage(_hittedDamage);
        }
        Destroy(this.gameObject);
    }
    public override void Damage(float damage)
    {
        _mode = 2;
        setTarget(MotherBoss.transform.position);
    }
}
