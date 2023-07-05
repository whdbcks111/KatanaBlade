using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Player player;
    private Rigidbody2D _rigid;

    private bool _dashCan;

    private int _stare;

    private bool _isJump;
    public float _Jump;
    public float _MaxSpeed;


    [SerializeField] private float _parryRadius;
    private void Start()
    {
        player = GetComponent<Player>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerJump();
    }

    private void FixedUpdate()
    {
        PlayerMove();

    }

    // 플레이어 움직임 구현
    private void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");

        _rigid.AddForce(Vector2.right * h * player.Stat.Get(StatType.MoveSpeed), ForceMode2D.Impulse);

        if (_rigid.velocity.x > _MaxSpeed)
        {
            _rigid.velocity = new Vector2(_MaxSpeed, _rigid.velocity.y);
        }

        else if (-_rigid.velocity.x > _MaxSpeed)
        {
            _rigid.velocity = new Vector2(-_MaxSpeed, _rigid.velocity.y);
        }
    }

    // 플레이어 점프 구현
    private void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && !_isJump)
        {
            _isJump = true;
            _rigid.velocity = new Vector2(_rigid.velocity.x, _Jump);
        }
    }

    private void PlayerParry()
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
                //적이 해당 방향/범위 안에 있는지 확인
                float MonsterDirection = ExtraMath.DirectionToAngle(inst.transform.position - transform.position);
                //해당 각도에 오차범위 추가
                //패링 성공
                //몬스터가 공격중인지 판단해서 패링 성공인지 판단
                if (Mathf.Abs(parryDirection - MonsterDirection) < 12)
                {
                    //패링 성공 이후 공격
                    t.Damage(player.Stat.Get(StatType.ParryingAttackForce));
                    //패링 후 효과
                    if (t is MeleeMonster)
                    {
                        //근거리의 경우 뒤로 밀림
                    }
                    //원거리면 피드백X

                }



            }

        }

        player.DashStamina -= player.Stat.Get(StatType.parry);
        StartCoroutine(ParryCool());
    }

    IEnumerator ParryCool()
    {
        yield return new WaitForSeconds(player.Stat.Get(StatType.ParryingTime));
    }

    private void StaminaGen()
    {

    }

    private void PlayerDash()
    {
        //가능한 상황인지 확인
        if (_dashCan)
        {
            //레이캐스트 쏘기
            Debug.DrawRay(transform.position, new Vector3(_stare * player.Stat.Get(StatType.DashLength), 0, 0), Color.green, 0.7f);
            //레이어 마스크는 추후에 유니티 엔진에서 추가하고 코드에도 추가
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(_stare, 0), player.Stat.Get(StatType.DashLength));
            //레이캐스트 닿으면
            if (hit.collider != null)
                transform.position = new Vector2(hit.transform.position.x, transform.position.y);
            //아니면 이동
            else
                transform.Translate(new Vector2(_stare * player.Stat.Get(StatType.DashLength), 0));
            _dashCan = false;
            StartCoroutine(DashCool());
        }
    }

    IEnumerator DashCool()
    {
        yield return new WaitForSeconds(player.Stat.Get(StatType.DashCooldown));
        _dashCan = true;
    }

    // 플레이어가 바닥에서만 점프하도록 구현
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJump = false;
        }
    }
}
