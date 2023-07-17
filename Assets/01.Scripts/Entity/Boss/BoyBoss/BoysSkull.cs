using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoysSkull : Boss
{
    public BoyBoss Boy;
    private Animator _animator;

    private static float _limitDistance = 5f;

    private static float _walkSpeed = 7f;
    private static float _dashSpeed = 13f;
    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        StartCoroutine(PatternTerm());
        print("hp : " + HP);
    }

    private void FloorCheck()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, LayerMask.GetMask("Platform"));
        if (hit.collider != null)
        {
            transform.position = new Vector2(transform.position.x, hit.collider.transform.position.y + 5.0f);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void AIAct()
    {
        switch (_aiMode)
        {
            case 0:
                Staring();
                _distance = Mathf.Abs(_player.transform.position.x - transform.position.x);
                //print("distance : " + _distance + "   Limit distance : " + _limitDistance);
                if (_distance > _limitDistance)
                    MovingVelocity = _stare * _walkSpeed;
                else
                    MovingVelocity = 0;
                WallCheck();
                _animator.SetFloat("Move", Mathf.Abs(MovingVelocity));

                break;
            case 1:
                StartCoroutine(Slash());
                break;
            case 2:
                StartCoroutine(Rush());
                break;
            case 3:
                MovingVelocity = _stare * _dashSpeed;
                WallCheck();
                break;
        }
    }
    IEnumerator Slash()
    {
        IsActable = false;
        MovingVelocity = 0;
        _animator.SetBool("Slash", true);
        yield return new WaitForSeconds(.4f);

        IsAttcking = true;
        yield return new WaitForSeconds(.6f);
        _animator.SetBool("Slash", false);
        StartCoroutine(PatternTerm());
    }
    IEnumerator Rush()
    {
        _animator.SetBool("Rush", true);
        _aiMode = 3;
        IsAttcking = true;
        yield return new WaitForSeconds(3.5f);
        _animator.SetBool("Rush", false);
        StartCoroutine(PatternTerm());
    }
    IEnumerator PatternTerm()
    {
        _aiMode = 0;
        IsAttcking = false;
        IsActable = true;
        yield return new WaitForSeconds(3.0f);
        NextPattern();
    }
    private void NextPattern()
    {
        if (!Boy.IsActing)
        {
            if (_distance < _player.Stat.Get(StatType.DashLength) * .5f)
                _aiMode = 1;
            else
                _aiMode = Mathf.FloorToInt(Random.Range(1, 3));
        }
        else
            StartCoroutine(PatternTerm());
        print("Pattern num " + _aiMode);
    }
    IEnumerator hit()
    {
        IsActable = false;
        MovingVelocity = 0;
        _animator.SetBool("Rush", false);
        _animator.SetBool("Slash", false);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(PatternTerm());
    }
    public override void Damage(float damage)
    {
        HP -= damage + _player.Stat.Get(StatType.BossAttackForce);
        print("hp : " + HP);
        if (IsAttcking)
        {
            StopAllCoroutines();
            StartCoroutine(hit());
        }
        _animator.SetTrigger("Hit");
        if (HP <= 0)
        {
            IsActable = false;
            MovingVelocity = 0;
            StartCoroutine(Death());
        }

    }
    IEnumerator Death()
    {
        _animator.SetBool("Dead", true);
        Boy.Damage(5f);
        yield return new WaitForSeconds(1.0f);
        base.OnMonsterDie();
    }
}
