using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfStorm : Item
{
    private static readonly float Cooldown = 5f;

    private static readonly float MaintainTime = 2;
    private static readonly float ActivePower = 2.5f;
    private static readonly float SkillWidth = 7.5f;

    public EssenceOfStorm()
: base(ItemType.Essence, "폭풍의 정수",
    string.Format(
        "사용 시 : 주변에 폭풍을 일으켜 적들을 {0}초간 <color=skyblue>띄우고</color> 기절시킵니다. <color=gray>(재사용 대시기간 : {1:0.0}초)</color>\n" +
        "기본 지속 효과 : - ", MaintainTime,Cooldown),
    Resources.Load<Sprite>("Item/Icon/Essence/Essence_0"))
    {
    }

    [ContextMenu("액티브 사용")]
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
        while(totalTime < maintainTime)
        {
            Collider2D[] area = Physics2D.OverlapBoxAll(Player.Instance.transform.position, new Vector2(SkillWidth * 2, 5f), 0f, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (var enemy in area)
            {
                if (enemy.GetComponent<Entity>() is Monster)
                {
                    //대상과의 거리에 따라 에어본이 달라짐
                    enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.up * ActivePower, ForceMode2D.Impulse);
                    enemy.GetComponent<Entity>().AddEffect(new EffectStun(1, 0.1f, Player.Instance));
                }
            }
            totalTime += .1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
