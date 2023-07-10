using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private Player _player;
    private Rigidbody2D _rigid;
    private int _platformLayer;

    private bool _dashCan;
    private bool _parryCan;

    private int _stare;

    private bool _canJump = false;

    private Collider2D _collider2D;

    public bool IsOnGround { get; private set; }


    [SerializeField] private float _parryRadius;
    [SerializeField] private GameObject _alter;

    public bool IsConscious { get; set; }
    private void Start()
    {
        IsConscious = true;
        _animator = GetComponentInChildren<Animator>();
        _player = GetComponent<Player>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _platformLayer = LayerMask.NameToLayer("Platform");
        StartCoroutine(ParryCool());
        StartCoroutine(DashCool());
    }

    private void Update()
    {
        if (IsConscious)
        {
            PlayerJump();
            StaminaGen();
            PlayerParry();
            PlayerDash();
            PlayerMove();

            FallingAnim();
        }
    }

    // 플레이어 움직임 구현
    private void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");


        _player.MovingVelocity = h * _player.Stat.Get(StatType.MoveSpeed);

        if (Mathf.Abs(h) > Mathf.Epsilon)
        {
            _stare = h > 0 ? 1 : -1;
            _animator.SetBool("Running", true);
        }
        else
            _animator.SetBool("Running", false);

        print(GetComponentInChildren<SpriteRenderer>());
        if (_stare < 0)
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        else
            GetComponentInChildren<SpriteRenderer>().flipX = false;
    }

    // 플레이어 점프 구현
    private void PlayerJump()
    {
        if (Input.GetButton("Jump") && _canJump)
        {
            _animator.SetBool("Jump", true);
            _canJump = false;
            _rigid.velocity = new Vector2(_rigid.velocity.x, _player.Stat.Get(StatType.JumpForce));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        IsOnGround = false;
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (_collider2D.bounds.min.y + 0.1f >= collision.GetContact(i).point.y)
            {
                IsOnGround = true;
                break;
            }
        }
        if ((collision.gameObject.layer & _platformLayer) != 0 && IsOnGround)
        {
            _canJump = true;
            _animator.SetBool("Falling", false);
            _animator.SetBool("Jump", false);
        }

    }
    private void FallingAnim()
    {
        if (_rigid.velocity.y < -1.0f)
        {
            _animator.SetBool("Falling", true);
            _animator.SetBool("Jump", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & _platformLayer) != 0)
        {
            IsOnGround = false;
            _canJump = false;
        }
    }

    private void PlayerParry()
    {
        if (_parryCan && Input.GetMouseButtonDown(0) && _player.DashStamina >= _player.Stat.Get(StatType.ParryingCost))
        {

            _animator.SetTrigger("Parry");
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
                        //몬스터가 공격중인지 판단해서 패링 성공인지 판단
                        if (t.TryGetComponent(out MeleeMonster m) && m.CanParrying)
                        {
                            //패링 성공 이후 공격
                            t.Damage(_player.Stat.Get(StatType.ParryingAttackForce));
                            //패링 후 효과

                        }


                        //패링 성공 이후 공격
                        //t.Damage(_player.Stat.Get(StatType.ParryingAttackForce));



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

            StartCoroutine(Stun(.6f));
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
            _player.DashStamina += Time.deltaTime * _player.Stat.Get(StatType.DashStaminaRegen);
        else
            _player.DashStamina = _player.MaxDashStamina;
        if (_player.ParryingStamina < _player.MaxParryingStamina)
            _player.ParryingStamina += Time.deltaTime * _player.Stat.Get(StatType.ParryingStaminaRegen);
        else
            _player.ParryingStamina = _player.MaxParryingStamina;
    }

    private void PlayerDash()
    {
        //가능한 상황인지 확인
        if (_dashCan && Input.GetKeyDown(KeyCode.LeftShift) && _player.DashStamina >= _player.Stat.Get(StatType.DashCost))
        {
            var pos = new Vector2(transform.position.x, _collider2D.bounds.min.y + 0.1f);
            var hit = Physics2D.Raycast(pos, Vector2.right * _stare, _player.Stat.Get(StatType.DashLength), LayerMask.GetMask("Platform"));
            float targetX;

            if(hit.collider is not null) targetX = hit.point.x - _stare * _collider2D.bounds.size.x / 2;
            else targetX = transform.position.x + _player.Stat.Get(StatType.DashLength) * _stare;

            GenerateAlter(transform.position.x, targetX);
            transform.position = new(targetX, transform.position.y);

            _player.DashStamina -= _player.Stat.Get(StatType.DashCost);
            StartCoroutine(DashCool());
        }
    }
    public void GenerateAlter(float startPos, float endPos)
    {
        float startX = Mathf.Min(startPos, endPos);
        float endX = Mathf.Max(startPos, endPos);
        SpriteRenderer childRenderer = GetComponentInChildren<SpriteRenderer>();
        for (float x = startX; x < endX; x += 1.2f)
        {
            GameObject copy = Instantiate(_alter, new Vector3(x, childRenderer.transform.position.y), _alter.transform.rotation);
            copy.GetComponent<SpriteRenderer>().flipX = childRenderer.flipX;
            copy.GetComponent<SpriteRenderer>().sprite = childRenderer.sprite;
            copy.transform.localScale = transform.localScale;
            Destroy(copy, 1.0f);
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

    IEnumerator Stun(float stunSec)
    {
        print("stun");
        IsConscious = false;
        yield return new WaitForSeconds(stunSec);
        IsConscious = true;
    }
}
