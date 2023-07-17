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
