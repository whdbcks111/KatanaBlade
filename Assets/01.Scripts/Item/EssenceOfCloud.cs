using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfCloud : Item
{

    private static readonly float PassiveTick = 2;
    private static readonly float PassiveForce = 3f;
    private static readonly float PassiveRadius = 5f;

    private static readonly float ActiveForce = 3f;
    private static readonly float Cooldown = 5f;
    private float _dT;

    public EssenceOfCloud()
        : base(ItemType.Essence, "������ ����",
            string.Format(
                "��� �� : <color=skyblue>����</color>�մϴ�. <color=#aaa>(���� ��ñⰣ : {0:0.0}��)</color>\n" +
                "�⺻ ���� ȿ�� : {1}�ʸ��� �ֺ� ���� �о���ϴ�.", Cooldown, PassiveTick),
            Resources.Load<Sprite>("Item/Icon/Essence/Essence_8"))
    {
    }

    public override void OnActiveUse()
    {
        Player.Instance.SetEssenceCooldown(Cooldown);

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
                    float dir = Player.Instance.transform.position.x > enemy.transform.position.x ? -1f : 1f;
                    enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.up * PassiveForce, ForceMode2D.Impulse);
                    enemy.GetComponent<Monster>().Knockback(dir * PassiveForce);
                }
            }
            _dT = 0;
        }
    }

    public override void OnMount()
    {
    }

    public override void OnUnmount()
    {
    }
}
