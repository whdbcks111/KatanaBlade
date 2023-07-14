
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeMonster : Monster
{
    protected Coroutine sco = null;

    public static MeleeMonster Instance { get; private set; }

    [SerializeField] protected bool _isAttacking;
    protected int _nextMove;
    private float _changeTimer = 0f;
    protected float _attackSpeed = 1f;

    public bool CanParrying { get { return _isAttacking; } }

    private bool _inRange;

    private void Start()
    {
        Think();

        Invoke("Think", 5);
    }

    protected override void Update()
    {
        base.Update();

        print(CanParrying);

        Stat.Multiply(StatType.MoveSpeed, 0.5f);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_inRange)
        {
            Chacing();
        }
        MonsterMove();

    }

    protected virtual void MonsterMove()
    {
        MovingVelocity = _isAttacking || HasEffect<EffectStun>() ? 0 : _nextMove * Stat.Get(StatType.MoveSpeed);
        var originScale = transform.localScale;
        if (_nextMove * originScale.x > 0f) originScale.x *= -1;
        transform.localScale = originScale;

        Vector2 frontVec = (Vector2)transform.position + Vector2.right * _nextMove;

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 4, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 4, LayerMask.GetMask("Platform"));
        
        if (rayHit.collider == null && _changeTimer <= 0f)
        {
            _nextMove *= -1;
            _changeTimer = 0.5f;
        }

        if (_changeTimer > 0f) _changeTimer -= Time.deltaTime;

    }

    
    private void Chacing()
    {
        var dir = (Player.Instance.transform.position - transform.position);

        var distance = dir.magnitude;

        if (distance <= 10.0f)
        {
            _nextMove = dir.x > 0 ? 1 : -1; 
        }

        else
        {
            Heal(1f * Time.deltaTime);
        }
    }
    
    private void Think()
    {
        _nextMove = Random.Range(-1, 2);
    }

    private void OnTriggerStay2D(Collider2D other)
    {

            if (other.TryGetComponent(out Player p))
            {
                _inRange = true;
                StartAttack(p);
            }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.TryGetComponent(out Player p))
        {
            _inRange = false;
        }
    }

    public virtual void StartAttack(Player other)
    {

        if (_isAttacking) return;
        _isAttacking = true;

        if (sco is null)
            sco = StartCoroutine(CountAttackDelay(other));
    }

    IEnumerator CountAttackDelay(Player other)
    {
       
//        yield return new WaitForSeconds(0.4f);
        yield return new WaitForSeconds(0.4f / _attackSpeed);

        _isAttacking = true;
        
        if (_inRange) Attack(other);
        print(other.HP);

//       yield return new WaitForSeconds(0.6f);
        yield return new WaitForSeconds(0.6f / _attackSpeed);

        _isAttacking = false;

        sco = null;
       
    }
}
