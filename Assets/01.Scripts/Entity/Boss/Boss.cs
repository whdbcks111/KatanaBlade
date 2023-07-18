using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    public string BossName = "º¸½º";
    public bool IsActable;
    protected float _distance;
    protected int _aiMode;
    protected Player _player;
    public bool IsAttcking;
    protected int _stare;
    protected Collider2D _col2d;
    protected virtual void Start()
    {
        _player = Player.Instance;
        _col2d = GetComponent<Collider2D>();

        GameManager.instance.BossHPBar.gameObject.SetActive(true);
        GameManager.instance.BossName.SetText(BossName);
    }

    protected override void Update()
    {
        base.Update();
        GameManager.instance.BossHPBar.normalizedValue = HP / MaxHP;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (IsActable)
            AIAct();
    }
    protected void Staring()
    {
        if (_player.transform.position.x < transform.position.x)
        {
            _stare = -1;
            //_renderer.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        else
        {
            _stare = 1;
            //_renderer.flipX = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        //print(_stare);
    }
    protected void WallCheck()
    {
        if (MovingVelocity == 0)
            return;

        Debug.DrawRay(transform.position, new Vector2(MovingVelocity, 0), Color.blue);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(MovingVelocity, 0), 4, LayerMask.GetMask("Platform"));
        if (hit.collider != null)
            MovingVelocity = 0;
    }
    public virtual void AIAct()
    {

    }
    public override void Damage(float damage)
    {
        base.Damage(damage);
    }

    private void OnDestroy()
    {

        GameManager.instance.BossHPBar.gameObject.SetActive(false);
    }
}
