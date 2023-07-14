using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulBoss : Boss
{
    [SerializeField] private GameObject _knifeTrap;


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
        Debug.DrawRay(transform.position, Vector2.down, Color.blue);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, LayerMask.GetMask("Platform"));
        if (hit.collider != null)
        {
            transform.position = new Vector2(transform.position.x, hit.collider.transform.position.y + 5.0f);
        }
    }
    public override void AIAct()
    {
        switch (_aiMode)
        {
            //������ �ִ� ������
            //�÷��̾� �ִ� ���� ó�ٺ���
            //���� ������ ���� �÷��̾�� �ǽð����� �Ÿ� ��α�
            //���Ͽ� ���� �ʿ��ϴ� �����ϰ� �÷��̾� ���� or �ݴ�������� �̵��ϱ�
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
            //���� 1
            case 1:
                StartCoroutine(Spin());
                break;
            //����1 ����
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
        //�ٰŸ�
        //Į ���ϸ�
        if (_distance < _player.Stat.Get(StatType.DashLength) * 0.8f)
            maxLimit = 2;
        //���Ÿ�
        //Į�� ����
        else if (_distance > _player.Stat.Get(StatType.DashLength) * 4.5f)
            minLimit = 2;
        //�߰Ÿ�
        //���� ��
        _aiMode = Mathf.FloorToInt(Random.Range(minLimit, maxLimit));

        //��� ���¿��� �÷��̾�� �ٰ�����, �־����� �Ǵ�

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
        IsActable = false;
        MovingVelocity = 0;
        _animator.SetBool("Spinning", false);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(PatternTerm());
    }
    public override void Damage(float damage)
    {
        base.Damage(damage * _player.Stat.Get(StatType.BossAttackForce));
        //���� ���߱�� �� �κп��� �˾Ƽ� ó��
        _animator.SetTrigger("Hitted");


        if (HP < 0)
        {
            //�߾� ���� ����
        }
        else if (IsAttcking)
        {
            StopAllCoroutines();
            StartCoroutine(hit()); 
        }
    }
}
/*
 * ����1: ȸ��. �ٰ߱Ÿ��϶� ������ ����.
 * ����2: Į�� �ٴڿ��� �ö���� ����. �ִϸ��̼����� ó��. �ٰŸ� or ���Ÿ��϶� ����ϴ� ����     
 * ����3: 
 */