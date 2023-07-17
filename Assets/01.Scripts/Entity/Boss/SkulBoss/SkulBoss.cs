using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulBoss : Boss
{
    [SerializeField] private KnifeStepper _knifeTrap;
    [SerializeField] private GameObject _trapPivot;


    private Animator _animator;

    //private float _moveSpeed;


    private static float _moveSpeed = 10f;

    private static float _NoNoRange = 5f;
    private static float _maxlimitRange = 10f;

    [SerializeField] BossAttackProjectile _knifePrefab;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        this.Stat.SetDefault(StatType.MaxHP, 700);
        HP = MaxHP;
        _animator = GetComponentInChildren<Animator>();
        FloorCheck();
        StartCoroutine(PatternTerm());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void FloorCheck()
    {
        Debug.DrawRay(transform.position, Vector2.left, Color.blue);
        RaycastHit2D wallHitL = Physics2D.Raycast(transform.position, Vector2.left, 300f, LayerMask.GetMask("Platform"));
        if (wallHitL.collider != null)
            _knifeTrap.MinLimit=wallHitL.collider.transform.position.x;
        Debug.DrawRay(transform.position, Vector2.right, Color.blue);
        RaycastHit2D wallHitR = Physics2D.Raycast(transform.position, Vector2.right, 300f, LayerMask.GetMask("Platform"));
        if (wallHitR.collider != null)
            _knifeTrap.MaxLimit = wallHitR.collider.transform.position.x;

        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, LayerMask.GetMask("Platform"));
        if (hit.collider != null)
        {
            print(hit.collider);
            transform.position = new Vector2(transform.position.x, hit.collider.transform.position.y + 5.0f);
            _trapPivot.transform.position = new Vector2(transform.position.x, hit.collider.transform.position.y);
        }
    }
    public override void AIAct()
    {
        switch (_aiMode)
        {
            //가만히 있는 대기상태
            //플레이어 있는 방향 처다보기
            //다음 패턴을 위해 플레이어와 실시간으로 거리 재두기
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
                WallCheck();
                break;
            //패턴 1
            case 1:
                StartCoroutine(Spin());
                break;
            //패턴1 추적
            case 5:
                MovingVelocity = _stare * _moveSpeed * 1.2f;
                WallCheck();
                break;
            case 2:
                StartCoroutine(KnifeStepping());
                break;
            case 3:
                StartCoroutine(KnifeThrowing());
                break;
        }

    }
    public void ChooseNextAct()
    {
        int minLimit = 1;
        int maxLimit = 4;
        //근거리
        //칼 패턴만
        if (_distance < _player.Stat.Get(StatType.DashLength) * 0.8f)
            maxLimit = 2;
        //원거리
        //칼만 제외
        else if (_distance > _player.Stat.Get(StatType.DashLength) * 4.5f)
            minLimit = 2;
        //중거리
        //전부 다
        _aiMode = Mathf.FloorToInt(Random.Range(minLimit, maxLimit));

        //대기 상태에서 플레이어에게 다가갈지, 멀어질지 판단

    }

    IEnumerator Spin()
    {
        IsAttcking = true;
        _animator.SetBool("Spinning", true);
        _aiMode = 5;

        yield return new WaitForSeconds(4.0f);
        _animator.SetBool("Spinning", false);
        IsAttcking = false;
        StartCoroutine(PatternTerm());
    }
    IEnumerator KnifeStepping()
    {
        MovingVelocity = 0;
        IsActable = false;
        _knifeTrap.GetComponent<KnifeStepper>().Shuffle();
        _knifeTrap.GetComponent<Animator>().SetTrigger("Shoot");
        yield return new WaitForSeconds(3.0f);
        _knifeTrap.GetComponent<Animator>().SetTrigger("Shoot");
        yield return new WaitForSeconds(2.0f);
        _knifeTrap.GetComponent<Animator>().SetTrigger("Shoot");
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(PatternTerm());
    }
    IEnumerator KnifeThrowing()
    {
        MovingVelocity = 0;
        IsActable = false;
        float x = Random.Range(2, 5);
        for (int i = 0; i < x; i++)
        {
            BossAttackProjectile copy = Instantiate(_knifePrefab, new Vector2(transform.position.x, transform.position.y + 5.0f), transform.rotation);
            copy.MotherBoss = this;
            yield return new WaitForSeconds(2.0f);
            copy.Fire();
            yield return new WaitForSeconds(1.5f);
        }
        StartCoroutine(PatternTerm());
    }
    IEnumerator PatternTerm()
    {
        _aiMode = 0;
        IsActable = true;
        yield return new WaitForSeconds(3.0f);
        ChooseNextAct();
    }
    IEnumerator hit()
    {
        IsAttcking = false;
        IsActable = false;
        MovingVelocity = 0;
        _animator.SetBool("Spinning", false);
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(PatternTerm());
    }
    public override void Damage(float damage)
    {
        base.Damage(damage);
        //패턴 멈추기는 각 부분에서 알아서 처리
        _animator.SetTrigger("Hitted");


        if (HP < 0)
        {
            //발악 패턴 시작
        }
        else if (IsAttcking)
        {
            StopAllCoroutines();
            StartCoroutine(hit());
        }
    }
}
/*
 * 패턴1: 회전. 중근거리일때 나오는 패턴.
 * 패턴2: 칼이 바닥에서 올라오는 패턴. 애니메이션으로 처리. 근거리 or 원거리일때 사용하는 패턴     
 * 패턴3: 
 */