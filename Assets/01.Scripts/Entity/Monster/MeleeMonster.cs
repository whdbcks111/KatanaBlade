
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeMonster : Entity
{
    Rigidbody2D _rigid;

    private bool _isAttacking;
    protected int _nextMove;
    private float _changeTimer = 0f;

    public bool IsParrying { get { return _isAttacking; } }

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        Think();

        Invoke("Think", 5);
    }

    private void FixedUpdate()
    {
        if (!_isAttacking)
        {
            Chacing();
            MonsterMove();
        }

    }


    protected virtual void MonsterMove()
    {
        transform.position += new Vector3(_nextMove, _rigid.velocity.y) * Time.fixedDeltaTime;
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
    }

    private void Think()
    {
        _nextMove = Random.Range(-2, 3);
    }



    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.TryGetComponent(out Player p))
        {
            StartAttack(p);
        }
    }

    public virtual void StartAttack(Player other)
    {
        if (_isAttacking) return;
        _isAttacking = true;
        StartCoroutine(CountAttackDelay(other));
    }

    IEnumerator CountAttackDelay(Player other)
    {
        yield return new WaitForSeconds(0.4f);

        Attack(other);

        yield return new WaitForSeconds(0.6f);
        
        _isAttacking = false;


    }
}
