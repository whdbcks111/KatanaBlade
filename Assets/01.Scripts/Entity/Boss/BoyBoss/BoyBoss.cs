using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyBoss : Boss
{
    [SerializeField]private BoysSkull _boySkulPrefab;
    private static float _NoNoRange = 5f;
    private static float _maxlimitRange = 10f;

    private static float _moveSpeed = 8f;

    private Animator _animator;
    protected override void Start()
    {

    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
                _animator.SetFloat("Move", MovingVelocity);
                WallCheck();
                break;
            case 10:
                StartCoroutine(Summon());
                break;
        }

    }
    IEnumerator Summon()
    {
        _animator.SetBool("Casting", true);
        yield return new WaitForSeconds(0.2f);
        //Instantiate(_boySkulPrefab);
        yield return new WaitForSeconds(2.5f);
    }
}
