
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeMonster : Monster
{
    protected Coroutine sco = null;

    [SerializeField] private float _jumpCoolTime;



    public static MeleeMonster Instance { get; private set; }

    [SerializeField] protected bool _isAttacking;
    protected int _nextMove;
    private float _changeTimer = 0f;
    protected float _attackSpeed = 1f;
    public float _jumpPower;

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

//      MovingVelocity = _jumpCoolTime > 0 || _isAttacking || HasEffect<EffectStun>() ? 0 : _nextMove * Stat.Get(StatType.MoveSpeed);
        var originScale = transform.localScale;
        if (_nextMove * originScale.x > 0f) originScale.x *= -1;
        transform.localScale = originScale;

        Vector2 frontVec = (Vector2)transform.position + Vector2.right * _nextMove;

        Debug.DrawRay(frontVec, Vector2.down, new Color(0, 4, 0));
        RaycastHit2D raHitGround = Physics2D.Raycast(frontVec, Vector3.down, 4, LayerMask.GetMask("Platform"));

        var col = GetComponent<Collider2D>();
        Debug.DrawRay(transform.position, new Vector2(_nextMove, 0), new Color(0, 4, 0));

                RaycastHit2D rayHitWall = Physics2D.BoxCast(transform.position, col.bounds.size, 0, Vector3.right * _nextMove, 
                                                    col.bounds.size.x / 2 + 0.5f, LayerMask.GetMask("Platform"));

        if (raHitGround.collider == null && _changeTimer <= 0f)
        {
            _nextMove *= -1;
            _changeTimer = 0.5f;
            _jumpCoolTime = 0;
        }

        _jumpCoolTime += Time.deltaTime;
        if (rayHitWall.collider != null)
        {
            if (_jumpCoolTime >= 1.5f && Physics2D.BoxCast(transform.position, col.bounds.size, 0, 
                Vector3.down, 0.3f, LayerMask.GetMask("Platform")).collider != null)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * _jumpPower;
                print("¤¸¤½~");
                _jumpCoolTime = 0;
            }
        }


        if (_changeTimer > 0f) _changeTimer -= Time.deltaTime;
        MovingVelocity = _isAttacking || HasEffect<EffectStun>() ? 0 : _nextMove * Stat.Get(StatType.MoveSpeed);
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
