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

        player.Stat.Get(StatType.ParryingAttackForce);
    }

    IEnumerator ParryCool()
    {
        yield return new WaitForSeconds(player.Stat.Get(StatType.ParryingTime));
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
