using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfCloud : Item
{

    private static readonly float PassiveTick = 2;
    private static readonly float PassiveForce = 3f;
    private static readonly float PassiveRadius = 5f;

    private static readonly float ActiveForce = 3f;
    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;
    private float _dT;

    public EssenceOfCloud()
        : base(ItemType.Essence, "구름의 정수",
            string.Format(
                "사용 시 : <color=skyblue>도약</color>합니다. <color=gray>(재사용 대시기간 : {0:0.0}초)</color>\n" +
                "기본 지속 효과 : {1}초마다 주변 적을 밀어냅니다.", Cooldown, PassiveTick),
            Resources.Load<Sprite>("Item/Icon/EssenceOfRegeneration"))
    {
    }

    public override void OnActiveUse()
    {
        if (_lastUsed > 0 && (Time.realtimeSinceStartup - _lastUsed) < Cooldown) return;
        _lastUsed = Time.realtimeSinceStartup;

        Player.Instance.GetComponent<Rigidbody2D>().AddForce(Vector2.up * ActiveForce, ForceMode2D.Impulse);
    }

    public override void PassiveUpdate()
    {
        _dT += Time.deltaTime;
        if(_dT > PassiveTick)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(Player.Instance.transform.position, PassiveRadius);
            foreach (var enemy in enemies)
            {
                if (enemy.GetComponent<Entity>() is Monster)
                {
                    Debug.Log(enemy.name);
                    float dir = Player.Instance.transform.position.x > enemy.transform.position.x ? -1f : 1f;
                    enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.up * ActiveForce, ForceMode2D.Impulse);
                    enemy.GetComponent<Monster>().Knockback(dir * ActiveForce);
                }
            }
            _dT = 0;
        }
    }
}
