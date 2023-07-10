
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeMonster : Monster
{
    protected Coroutine sco = null;

    [SerializeField] protected bool _isAttacking;
    protected int _nextMove;
    private float _changeTimer = 0f;

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

    // 몬스터 움직임 코드
    protected virtual void MonsterMove()
    {
        MovingVelocity = _isAttacking ? 0 : _nextMove * Stat.Get(StatType.MoveSpeed);
        print("Base : " + MovingVelocity);

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

    // 몬스터가 플레이어 쫒도록 하는 코드
    private void Chacing()
    {
        var dir = (Player.Instance.transform.position - transform.position);

        var distance = dir.magnitude;

        if (distance <= 10.0f)
        {
            _nextMove = dir.x > 0 ? 1 : -1;
        }
    }

    private void Think()
    {
        _nextMove = Random.Range(-1, 2);
    }

    // 몬스터가 melee에 닿으면 공격하는 코드
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

    // 몬스터의 공격 딜레이 코드
    IEnumerator CountAttackDelay(Player other)
    {
      //  print("attackStart");
        yield return new WaitForSeconds(0.4f);

        if(_inRange) Attack(other);
        print(other.HP);
      //  print("Attacked");
        yield return new WaitForSeconds(0.6f);  

        _isAttacking = false;
        sco = null;
     //   print("attackEnd");

    }
}
