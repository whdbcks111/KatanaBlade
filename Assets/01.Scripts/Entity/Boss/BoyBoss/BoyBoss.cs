using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyBoss : Boss
{
    public bool IsActing;
    [SerializeField] private BoysSkull _boySkulPrefab;
    [SerializeField] private BoyCastBall castBall;

    private static float _NoNoRange = 5f;
    private static float _maxlimitRange = 15f;

    private static float _moveSpeed = 3f;

    private Animator _animator;
    private BoysSkull _summonned;
    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        Stat.SetDefault(StatType.MaxHP, 220);
        HP = MaxHP;
        StartCoroutine(PatternTerm());
        FloorCheck();
    }

    private void FloorCheck()
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, LayerMask.GetMask("Platform"));
        if (hit.collider != null)
        {
            transform.position = new Vector2(transform.position.x, hit.point.y + 2.1f);
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
            //가만히 있는 대기상태
            //플레이어 있는 방향 처다보기
            //패턴에 따라 필요하니 랜덤하게 플레이어 방향 or 반대방향으로 이동하기
            case 0:
                Staring();
                _distance = Mathf.Abs(_player.transform.position.x - transform.position.x);
                if (_distance > _maxlimitRange)
                    MovingVelocity = _stare * _moveSpeed;
                else if (_distance < _NoNoRange)
                    MovingVelocity = _stare * -_moveSpeed;
                else
                    MovingVelocity = 0;
                _animator.SetFloat("WalkSpeed", _stare * MovingVelocity);
                WallCheck();
                break;
            case 1:
                StartCoroutine(Summon());
                break;
            case 2:
                StartCoroutine(Cast());
                break;
        }

    }
    IEnumerator Cast()
    {
        IsActing = true;
        IsActable = false;
        MovingVelocity = 0;
        yield return new WaitForSeconds(.2f);
        _animator.SetBool("Casting", true);
        yield return new WaitForSeconds(.4f);
        BoyCastBall copyL = Instantiate(castBall, new Vector2(transform.position.x - 5f, transform.position.y + 3f), transform.rotation);
        copyL.MotherBoss = this;
        yield return new WaitForSeconds(.4f);
        BoyCastBall copyR = Instantiate(castBall, new Vector2(transform.position.x + 5f, transform.position.y + 3f), transform.rotation);
        copyR.MotherBoss = this;
        yield return new WaitForSeconds(.9f);
        copyL.Fire();
        yield return new WaitForSeconds(.9f);
        copyR.Fire();
        yield return new WaitForSeconds(1.8f);
        _animator.SetBool("Casting", false);
        yield return new WaitForSeconds(.4f);
        StartCoroutine(PatternTerm());
    }
    IEnumerator Summon()
    {
        IsActable = false;
        MovingVelocity = 0;
        yield return new WaitForSeconds(.4f);
        _animator.SetBool("Casting", true);
        yield return new WaitForSeconds(1.2f);
        _summonned = Instantiate(_boySkulPrefab, transform.position, transform.rotation);
        _summonned.Boy = this;
        _animator.SetBool("Casting", false);
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(PatternTerm());
    }
    IEnumerator PatternTerm()
    {
        IsActing = false;
        _aiMode = 0;
        IsActable = true;
        yield return new WaitForSeconds(6.0f);
        NextPattern();
    }
    private void NextPattern()
    {
        if (_summonned == null)
        {
            _aiMode = 1;
        }
        else
            _aiMode = 2;
    }
    public override void Damage(float damage)
    {
        _animator.SetTrigger("Hit");
        base.Damage(damage);
    }
    public override void OnMonsterDie()
    {
        _summonned.Damage(100);
        base.OnMonsterDie();
    }
}
