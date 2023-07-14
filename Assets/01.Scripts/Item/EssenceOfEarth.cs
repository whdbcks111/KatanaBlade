using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfEarth : Item
{
    private static readonly float Cooldown = 5f;
    private static readonly float PassiveTick = 5f;
    private static readonly float ActiveMag = 5f;
    private static readonly float SkillWidth = 7f;
    private float _dT;

    public EssenceOfEarth()
    : base(ItemType.Essence, "������ ����",
        string.Format(
            "��� �� : �ֺ� ������ <color=brown>�о��</color> ������ŵ�ϴ�. <color=gray>(���� ��ñⰣ : {0:0.0}��)</color>\n" +
            "�⺻ ���� ȿ�� : {1}�ʸ��� ������ ������ �ֺ� ���� <color=brown>����</color>��ŵ�ϴ�.", Cooldown, PassiveTick),
        Resources.Load<Sprite>("Item/Icon/Essence/Essence_0"))
    {
    }

    [ContextMenu("��Ƽ�� ���")]
    public override void OnActiveUse()
    {
        Player.Instance.SetEssenceCooldown(Cooldown);

        Collider2D[] area = Physics2D.OverlapBoxAll(Player.Instance.transform.position, new Vector2(SkillWidth * 2, 2f), 0f, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (var enemy in area)
        {
            if(enemy.GetComponent<Entity>() is Monster)
            {
                enemy.GetComponent<Entity>().AddEffect(new EffectStun(1, 2f, Player.Instance));
                //������ �Ÿ��� ���� ����� �޶���
                enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (ActiveMag + (SkillWidth - Vector2.Distance(enemy.transform.position, Player.Instance.transform.position))), ForceMode2D.Impulse);
                enemy.GetComponent<Entity>().Knockback(ActiveMag * Random.Range(-1, 2) * 2);
            }
        }
    }

    public override void PassiveUpdate()
    {
        _dT += Time.deltaTime;
        if(_dT > PassiveTick)
        {
            Collider2D[] area = Physics2D.OverlapBoxAll(Player.Instance.transform.position, new Vector2(SkillWidth * 2, 2f), 0f, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (var enemy in area)
            {
                enemy.GetComponent<Entity>().AddEffect(new EffectStun(1, .5f, Player.Instance));
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
