using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : Monster
{
    public float Speed;

    [Range(10f, 40f)]
    public float DetectDist;
    [Range(10f, 30f)]
    public float AttDist;       //공격 최대 거리
    [Range(5f, 15f)]
    public float MoveDist;      //공격 시 최소 사거리

    public float AttSpeed;

    public float JumpPow;

    public Transform JumpSenseTr;
    public Transform TileDetectTr;
    public GameObject ChargeImage;
    public GameObject BulletPrefab;
    public Animator ArcherAnimator;
    private Vector3 dir = Vector3.one;
    private Coroutine _attackCor;
    private Rigidbody2D _rb2d;
    private RaycastHit2D _ray;

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
            if (Vector2.Distance(transform.position, Player.Instance.transform.position) > MoveDist && _attackCor == null)
            {
                _ray = Physics2D.Raycast(TileDetectTr.position, Vector2.down, 2f, 1 << LayerMask.NameToLayer("Platform"));
                    /*Physics2D.OverlapCircle(TileDetectTr.position, 0.1f, 1 << LayerMask.NameToLayer("Platform"));*/
                if (_ray)
                {
                    ArcherAnimator.SetBool("Idle", false);
                    ArcherAnimator.SetBool("Walk", true);
                    ArcherAnimator.SetBool("Attack", false);
                }
                else
                {
                    ArcherAnimator.SetBool("Idle", true);
                    ArcherAnimator.SetBool("Walk", false);
                    ArcherAnimator.SetBool("Attack", false);
                }
                Move(Player.Instance);
            }
            else
            {
                ArcherAnimator.SetBool("Attack", true);
                ArcherAnimator.SetBool("Walk", false);
                ArcherAnimator.SetBool("Idle", false);
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
        if (moveTo.transform.position.x > transform.position.x)
        {
            dir = Vector3.one;
        }
        else
        {
            dir = new Vector3(-1, 1, 1);
        }
        transform.localScale = dir;

        if (_ray)
            transform.Translate(dir * Speed * Time.deltaTime);

        Collider2D obs = Physics2D.OverlapCircle(JumpSenseTr.position, 0.1f, 1 << LayerMask.NameToLayer("Platform"));
        if(obs != null && _rb2d.velocity.y == 0)
        {
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(JumpPow * Vector2.up, ForceMode2D.Impulse);
        }
    }

    public override void Attack(Entity other)
    {
        if (other.transform.position.x > transform.position.x)
        {
            dir = Vector3.one;
        }
        else
        {
            dir = new Vector3(-1, 1, 1);
        }
        transform.localScale = dir;
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
        bullet.Speed = 10f;
        bullet.SetOwner(this, ExtraMath.DirectionToAngle(Player.Instance.transform.position - transform.position));
        yield return new WaitForSeconds(0.1f);
        _attackCor = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(JumpSenseTr.position, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(TileDetectTr.position, 0.1f);
        Gizmos.DrawLine(TileDetectTr.position, TileDetectTr.position + (Vector3.down * 2));
        Gizmos.DrawWireSphere(TileDetectTr.position + (Vector3.down * 2), 0.1f);
    }

    public override void OnMonsterDie()
    {
        base.OnMonsterDie();
    }
}
