using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeMonster : Entity
{
    Rigidbody2D _rigid;
    Animator _anim;

    private bool _isAttacking;
    private int _nextMove;
    private float _changeTimer = 0f;

    public bool IsParrying { get { return _isAttacking; } }

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        Think();

        Invoke("Think", 5);
    }

    private void FixedUpdate()
    {
        if (!_isAttacking)
            MonsterMove();
  
    }

    // 몬스터 움직임 구현
    private void MonsterMove()
    {
        transform.position += new Vector3(_nextMove, _rigid.velocity.y) * Time.fixedDeltaTime;
        _anim.SetFloat("WalkSpeed", Mathf.Abs(_nextMove));
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

    // 몬스터가 랜덤으로 움직일 수 있는 범위 지정
    private void Think()
    {
        _nextMove = Random.Range(-2, 3);
    }



    // 범위 내에 들어오면 공격 
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.TryGetComponent(out Player p))
        {
            Attack(p);
        }
    }

    public override void Attack(Entity other)
    {
        if (_isAttacking) return;
        _isAttacking = true;
        _anim.SetTrigger("Attack");
        StartCoroutine(CountAttackDelay());
        base.Attack(other);
    }
    IEnumerator CountAttackDelay()
    {
        yield return new WaitForSeconds(0.4f);


        _isAttacking = false;
    }
}










