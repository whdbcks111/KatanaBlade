using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            PlayerDash();
            PlayerMove();
            PlayerParry();

            StareSet();
            FallingAnim();
        }
    }
    private void StareSet()
    {
        if (_stare < 0)
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        else
            GetComponentInChildren<SpriteRenderer>().flipX = false;
    }
    // �÷��̾� ������ ����
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

    }

    // �÷��̾� ���� ����
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
        if (_parryCan && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && _player.ParryingStamina >= _player.Stat.Get(StatType.ParryingCost))
        {
            var dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float parryAngle = ExtraMath.DirectionToAngle(dir);
            _animator.SetTrigger("Parry");
            StartCoroutine(ParryContinue(parryAngle));
            _stare = dir.x > 0 ? 1 : -1;
        }
    }
    IEnumerator ParryContinue(float parryAngle)
    {

        StartCoroutine(Stun(.6f));
        _player.ParryingStamina -= _player.Stat.Get(StatType.ParryingCost);
        StartCoroutine(ParryCool());
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Parry" &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 3 / 9);

        //�� �ݶ��̴� ����
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, _parryRadius, Vector2.zero);
        //�÷��̾�� ���콺 ���� �������ϱ�

        //������ ����, ����ü���� Ȯ��
        foreach (RaycastHit2D inst in hit)
        {
            //print(inst.collider.gameObject.name);
            float monsterAngle = ExtraMath.DirectionToAngle(inst.transform.position - transform.position);
            if (ExtraMath.IsAngleBetween(parryAngle, monsterAngle - 25, monsterAngle + 25))
            {
                if (inst.collider.TryGetComponent(out Entity t) && t is not Player)
                {
                    print(inst.collider.gameObject + " PARRYED");
                    //�ش� ������ �������� �߰�: �� 50���� ���� ���� ����
                    //�и� ����
                    //���Ͱ� ���������� �Ǵ��ؼ� �и� �������� 
                    //���Ͱ� ���������� �Ǵ��ؼ� �и� �������� �Ǵ�
                    if (t is MeleeMonster m && m.CanParrying)
                    {
                        //�и� ���� ���� ����
                        t.Damage(_player.Stat.Get(StatType.ParryingAttackForce));
                        //print("Melee Parry");
                        //�и� �� ȿ��
                        t.Knockback((t.transform.position.x > transform.position.x ? 1 : -1) * _player.Stat.Get(StatType.LowParryingFeedback));
                    }

                    else if (t is BossAttack)
                    {
                        //�и� ���� ���� ����
                        t.Damage(_player.Stat.Get(StatType.ParryingAttackForce));
                        //�и� �� ȿ��
                        _player.Knockback((t.transform.position.x > transform.position.x ? -1 : 1) * t.Stat.Get(StatType.HighParryingFeedback));
                        //t.Knockback((t.transform.position.x > transform.position.x ? 1 : -1) * _player.Stat.Get(StatType.LowParryingFeedback));
                    }
                    else if (t is Boss boss && boss.IsAttcking)
                    {
                        //�и� ���� ���� ����
                        t.Damage(_player.Stat.Get(StatType.ParryingAttackForce));
                        //�и� �� ȿ��
                        _player.Knockback((t.transform.position.x > transform.position.x ? -1 : 1) * t.Stat.Get(StatType.HighParryingFeedback));
                        t.Knockback((t.transform.position.x > transform.position.x ? 1 : -1) * _player.Stat.Get(StatType.LowParryingFeedback));
                    }
                    else if (inst.collider.TryGetComponent(out BossAttackProjectile bp))
                    {
                        bp.Damage(1);
                        _player.Knockback((t.transform.position.x > transform.position.x ? -1 : 1) * t.Stat.Get(StatType.MiddleParryingFeedback));
                    }
                }
                else if (inst.collider.TryGetComponent(out Projectile p))
                {
                    if (p is FlyingProjectile)
                        Destroy(p.gameObject);
                    //�и����� �ĳ���
                    //p.transform.Rotate(Vector3.forward * 180);
                    else
                    {
                        p.SetOwner(_player, parryAngle);
                    }
                }

            }
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
        //������ ��Ȳ���� Ȯ��
        if (_dashCan && Input.GetKeyDown(KeyCode.LeftShift) && _player.DashStamina >= _player.Stat.Get(StatType.DashCost))
        {
            var hit = Physics2D.BoxCast(_collider2D.bounds.center, (Vector2)_collider2D.bounds.size * .9f, 0, Vector2.right * _stare,
                _player.Stat.Get(StatType.DashLength), LayerMask.GetMask("Platform"));
            float targetX;

            if (hit.collider is not null) targetX = hit.point.x - _stare * _collider2D.bounds.size.x / 2;
            else targetX = transform.position.x + _player.Stat.Get(StatType.DashLength) * _stare;

            GenerateAlter(transform.position.x, targetX);
            transform.position = new(targetX, transform.position.y);

            _player.DashStamina -= _player.Stat.Get(StatType.DashCost);
            StartCoroutine(DashCool());
        }
    }
    public void GenerateAlter(float startPos, float endPos)
    {
        SpriteRenderer childRenderer = GetComponentInChildren<SpriteRenderer>();
        var span = 1.2f;
        int count = Mathf.FloorToInt(Mathf.Abs(endPos - startPos) / span);
        List<SpriteRenderer> renders = new List<SpriteRenderer>();
        for (int i = 0; i < count; i++)
        {

            GameObject copy = Instantiate(_alter, new Vector3(startPos + _stare * i * span, childRenderer.transform.position.y), _alter.transform.rotation);
            var renderer = copy.GetComponent<SpriteRenderer>();
            renderer.flipX = childRenderer.flipX;
            renderer.sprite = childRenderer.sprite;
            var col = renderer.color;
            col.a = Mathf.Clamp01((float)i / count + 0.3f);
            renderer.color = col;
            copy.transform.localScale = transform.localScale;
            renders.Add(renderer);
            StartCoroutine(AlphaDestroy(renderer));
        }
    }

    IEnumerator AlphaDestroy(SpriteRenderer renderer)
    {
        while (renderer.color.a > 0)
        {
            var c = renderer.color;
            c.a -= Time.deltaTime;
            renderer.color = c;
            yield return null;
        }
        Destroy(renderer.gameObject);
    }

    IEnumerator DashCool()
    {
        //�뽬 ���� ��� ���߰�
        _rigid.velocity = Vector2.zero;

        _dashCan = false;
        yield return new WaitForSeconds(_player.Stat.Get(StatType.DashCooldown));
        _dashCan = true;
    }

    IEnumerator Stun(float stunSec)
    {
        //print("stun");
        IsConscious = false;
        _player.MovingVelocity = 0;
        yield return new WaitForSeconds(stunSec);
        IsConscious = true;
    }
}
