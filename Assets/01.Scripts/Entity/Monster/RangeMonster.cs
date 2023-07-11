using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : Monster
{

    public float Speed;
    [Range(3f, 9f)]
    public float AttDist;
    [Range(2f, 7f)]
    public float MoveDist;

    public float AttSpeed;

    public float JumpPow;

    public Transform JumpSenseTr;
    public GameObject ChargeImage;
    public GameObject BulletPrefab;
    public Animator ArcherAnimator;
    private bool _flipX;
    private Coroutine _attackCor;
    private Rigidbody2D _rb2d;

    protected override void Awake()
    {
        base.Awake();
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
        if (AttDist < MoveDist)
            AttDist = MoveDist + 1;
    }
    

    protected override void Update()
    {
        base.Update();
        if(HasEffect<EffectStun>() == false)
        {
            if(Vector2.Distance(transform.position, Player.Instance.transform.position) > MoveDist && _attackCor == null)
            {
                ArcherAnimator.SetBool("Idle", false);
                ArcherAnimator.SetBool("Walk", true);
                Move(Player.Instance);
            }
            else
            {
                ArcherAnimator.SetBool("Attack", true);
                ArcherAnimator.SetBool("Walk", false);
                Attack(Player.Instance);
            }
        }
        else
        {
            ArcherAnimator.SetBool("Idle", true);
            ArcherAnimator.SetBool("Walk", false);
            ArcherAnimator.SetBool("Attack", false);
        }
    }

    private void Move(Entity moveTo)
    {
        if(moveTo.transform.position.x > transform.position.x)
        {
            transform.Translate(Vector2.right * Speed * Time.deltaTime);
            //_rb2d.velocity = new Vector2(Speed, _rb2d.velocity.y);
            _flipX = false;
        }
        else
        {
            transform.Translate(Vector2.left * Speed * Time.deltaTime);
            //_rb2d.velocity = new Vector2(-1 * Speed, _rb2d.velocity.y);
            _flipX = true;
        }
        transform.localScale = _flipX ? new Vector3(-1, 1, 1) : Vector3.one;
        Collider2D obs = Physics2D.OverlapCircle(JumpSenseTr.position, 0.1f, 1 << LayerMask.NameToLayer("Platform"));
        if(obs != null && _rb2d.velocity.y == 0)
        {
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(JumpPow * Vector2.up, ForceMode2D.Impulse);
        }
    }

    public override void Attack(Entity other)
    {
        //_rb2d.velocity = new Vector2(0, _rb2d.velocity.y);
        if (Vector2.Distance(transform.position, other.transform.position) > AttDist)
        {
            if (_attackCor != null)
            {
                ChargeImage.SetActive(false);
                StopCoroutine(_attackCor);
                _attackCor = null;
            }
        }
        else
        {
            if (_attackCor == null)
                _attackCor = StartCoroutine(AttackCor(AttSpeed));
        }
    }

    private IEnumerator AttackCor(float _attSpeed)
    {
        ChargeImage.SetActive(true);
        ChargeImage.transform.localScale = Vector3.one;
        ChargeImage.transform.eulerAngles = Vector3.zero;
        float dT = 0;
        while (true)
        {
            if (dT > 1 / _attSpeed)
                break;

            dT += Time.deltaTime;
            yield return null;

            ChargeImage.transform.localScale = Vector3.one * (1 - (dT / (1 / _attSpeed)));
            ChargeImage.transform.eulerAngles = new Vector3(0, 0, (dT / (1 / _attSpeed) * 360));
            if (HasEffect<EffectStun>())
            {
                ChargeImage.SetActive(false);
                StopCoroutine(_attackCor);
                _attackCor = null;
            }
        }
        ArcherAnimator.SetTrigger("Shot");
        ChargeImage.SetActive(false);
        ChargeImage.transform.localScale = Vector3.one;
        Projectile bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        bullet.Speed = 5f;
        bullet.SetOwner(this, ExtraMath.DirectionToAngle(Player.Instance.transform.position - transform.position));
        yield return new WaitForSeconds(0.1f);
        _attackCor = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(JumpSenseTr.position, 0.1f);
    }

    public override void OnMonsterDie()
    {
        base.OnMonsterDie();
    }
}
