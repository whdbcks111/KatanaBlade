using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfStorm : Item
{
    private static readonly float Cooldown = 10f;

    private static readonly float MaintainTime = 2;
    private static readonly float ActivePower = 15f;
    private static readonly float SkillWidth = 10f;

    public EssenceOfStorm()
: base(ItemType.Essence, "��ǳ�� ����",
    string.Format(
        "��� �� : �ֺ��� ��ǳ�� ������ ������ {0}�ʰ� <color=skyblue>����</color> ������ŵ�ϴ�. <color=gray>(���� ��ñⰣ : {1:0.0}��)</color>\n" +
        "�⺻ ���� ȿ�� : - ", MaintainTime,Cooldown),
    Resources.Load<Sprite>("Item/Icon/Essence/Essence_9"))
    {
    }

    [ContextMenu("��Ƽ�� ���")]
    public override void OnActiveUse()
    {
        Player.Instance.SetEssenceCooldown(Cooldown);
        Player.Instance.StartCoroutine(SkillCor(MaintainTime));
    }

    public override void OnMount()
    {
    }

    public override void OnUnmount()
    {
    }

    public override void PassiveUpdate()
    {
    }

    private IEnumerator SkillCor(float maintainTime)
    {
        float totalTime = 0;
        while(totalTime < maintainTime * Player.Instance.Stat.Get(StatType.EssenceForce))
        {
            Collider2D[] area = Physics2D.OverlapCircleAll(Player.Instance.transform.position, SkillWidth * 2, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (var enemy in area)
            {
                if (enemy.GetComponent<Entity>() is Monster)
                {
                    //������ �Ÿ��� ���� ����� �޶���
                    enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.up * ActivePower, ForceMode2D.Impulse);
                    enemy.GetComponent<Entity>().AddEffect(new EffectStun(1, 0.1f, Player.Instance));
                }
            }
            totalTime += .1f;
            Camera.main.GetComponent<CameraControll>().Shake(0.1f, Mathf.Clamp(totalTime, 0.1f, 1f));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
