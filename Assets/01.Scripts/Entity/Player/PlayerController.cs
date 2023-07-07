using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    private Rigidbody2D _rigid;
    private int _platformLayer;

    private bool _dashCan;
    private bool _parryCan;

    private int _stare;

    private bool _canJump = false;

    private Collider2D _collider2D;


    [SerializeField] private float _parryRadius;
    private void Start()
    {
        _player = GetComponent<Player>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _platformLayer = LayerMask.NameToLayer("Platform");
        StartCoroutine(ParryCool());
        StartCoroutine(DashCool());
    }

    private void Update()
    {
        PlayerJump();
        StaminaGen();
        PlayerParry();
        PlayerDash();
        PlayerMove();
    }

    // 플레이어 움직임 구현
    private void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");

        _player.MovingVelocity = h * _player.Stat.Get(StatType.MoveSpeed);

        if (Mathf.Abs(h) > Mathf.Epsilon)
            _stare = h > 0 ? 1 : -1;
    }
 
    // 플레이어 점프 구현
    private void PlayerJump()
    {
        if (Input.GetButton("Jump") && _canJump)
        {
            _canJump = false;
            _rigid.velocity = new Vector2(_rigid.velocity.x, _player.Stat.Get(StatType.JumpForce));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        bool isOnGround = false;
        for(int i = 0; i < collision.contactCount; i++)
        {
            if(_collider2D.bounds.min.y + 0.1f >= collision.GetContact(i).point.y)
            {
                isOnGround = true;
                break;
            }
        }
        if ((collision.gameObject.layer & _platformLayer) != 0 && isOnGround)
        {
            _canJump = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & _platformLayer) != 0)
            _canJump = false;
    }

    private void PlayerParry()
    {
        if (_parryCan && Input.GetMouseButtonDown(0))
        {
            //원 콜라이더 생성
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, _parryRadius, Vector2.zero);
            //플레이어와 마우스 사이 각도구하기
            float parryDirection = ExtraMath.DirectionToAngle(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            //닿은게 몬스터, 투사체인지 확인
            foreach (RaycastHit2D inst in hit)
            {

                if (inst.transform.TryGetComponent(out Entity t) && t is not Player)
                {
                    print(inst.collider.gameObject);
                    //적이 해당 방향/범위 안에 있는지 확인
                    float MonsterDirection = ExtraMath.DirectionToAngle(inst.transform.position - transform.position);
                    //해당 각도에 오차범위 추가: 약 50도의 오차 범위 존재
                    //패링 성공
                    //몬스터가 공격중인지 판단해서 패링 성공인지 판단
                    if (Mathf.Abs(parryDirection - MonsterDirection) < 25)
                    {
                        //패링 성공 이후 공격
                        t.Damage(_player.Stat.Get(StatType.ParryingAttackForce));
                        //패링 후 효과
                        //보스만 플레이어가 뒤로 밀리는걸로
                        /*if (t is MeleeMonster)
                        {
                            //오른쪽일 경우 ~90도, 270~
                            //왼쪽일 경우 90< 적정범위 <270
                            _rigid.velocity = new Vector2(_knockbackPlaceHolder * , 0);
                        }*/

                    }



                }

            }

            _player.DashStamina -= _player.Stat.Get(StatType.ParryingCost);
            StartCoroutine(ParryCool());
        }
    }

    IEnumerator ParryCool()
    {
        _parryCan = false;
        yield return new WaitForSeconds(_player.Stat.Get(StatType.ParryingTime));
        _parryCan = true;
    }

    private void StaminaGen()
    {
        if (_player.DashStamina < _player.MaxDashStamina)
            _player.DashStamina += Time.deltaTime * 3;
        else
            _player.DashStamina = _player.MaxDashStamina;
        if (_player.ParryingStamina < _player.MaxParryingStamina)
            _player.ParryingStamina += Time.deltaTime * 3;
        else
            _player.ParryingStamina = _player.MaxParryingStamina;
    }

    private void PlayerDash()
    {
        //가능한 상황인지 확인
        if (_dashCan && Input.GetKeyDown(KeyCode.LeftShift))
        {
            //레이캐스트 쏘기
            Debug.DrawRay(transform.position, new Vector3(_stare * _player.Stat.Get(StatType.DashLength), 0, 0), Color.green, 0.7f);
            //레이어 마스크는 추후에 유니티 엔진에서 추가하고 코드에도 추가
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(_stare, 0), _player.Stat.Get(StatType.DashLength), LayerMask.GetMask("DashStop"));
            //레이캐스트 닿으면
            if (hit.collider != null)
                transform.position = new Vector2(hit.transform.position.x + -_stare * .6f, transform.position.y);
            //아니면 이동
            else
                transform.Translate(new Vector2(_stare * _player.Stat.Get(StatType.DashLength), 0));

            _player.DashStamina -= _player.Stat.Get(StatType.DashCost);
            StartCoroutine(DashCool());
        }
    }

    IEnumerator DashCool()
    {
        //대쉬 직후 잠시 멈추게
        _rigid.velocity = Vector2.zero;

        _dashCan = false;
        yield return new WaitForSeconds(_player.Stat.Get(StatType.DashCooldown));
        _dashCan = true;
    }
}
