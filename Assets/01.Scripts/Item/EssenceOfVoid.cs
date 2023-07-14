using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfVoid : Item
{
    private static readonly float Cooldown = 10f;

    private static readonly float MaintainTime = 5;
    private static readonly float Radius = 6;
    private static readonly float CastTime = 0.2f;

    public EssenceOfVoid()
    : base(ItemType.Essence, "공허의 정수",
        string.Format(
            "사용 시 : 주변 투사체를 <color=#404>파괴</color>하는 영역을 생성합니다. <color=#aaa>(재사용 대시기간 : {0:0.0}초)</color>\n", Cooldown),
        Resources.Load<Sprite>("Item/Icon/Essence/Essence_3"))
    {

    }

    [ContextMenu("액티브 사용")]
    public override void OnActiveUse()
    {
        Player.Instance.SetEssenceCooldown(Cooldown);

        Player.Instance.StartCoroutine(SkillCor(MaintainTime, Radius, CastTime));
    }

    public override void PassiveUpdate()
    {
        //Player.Instance.Heal(Time.deltaTime * 2);
    }

    private IEnumerator SkillCor(float maintainTime, float radius, float castTime)
    {
        Sprite sprite = Resources.Load<Sprite>("Item/Icon/Area");
        GameObject effect = new GameObject();
        effect.AddComponent<SpriteRenderer>().sprite = sprite;
        effect.GetComponent<SpriteRenderer>().color = new Color(102f / 255f, 0, 102f/255f);
        effect.transform.localScale = Vector2.zero;
        effect.transform.position = Player.Instance.transform.position;

        //시전 코드
        float dT = 0;
        while(dT < castTime)
        {
            effect.transform.localScale = Vector2.one * radius * dT * (1f / castTime);
            yield return null;
            dT += Time.deltaTime;
        }
        //

        //영역 내 투사체 삭제
        effect.transform.localScale = Vector2.one * radius;
        dT = 0;

        while (dT < maintainTime)
        {
            Collider2D[] projectiles = Physics2D.OverlapCircleAll(Player.Instance.transform.position, radius);
            foreach (var b in projectiles)
            {
                if (b?.GetComponent<Projectile>() is Projectile)
                    Object.Destroy(b.gameObject);
            }
            yield return null;
            dT += Time.deltaTime;
        }
        //

        //시전 종료
        dT = castTime;

        while (dT > 0)
        {
            effect.transform.localScale = Vector2.one * radius * dT * (1f / castTime);
            yield return null;
            dT -= Time.deltaTime;
        }

        Object.Destroy(effect);
        //
    }

    public override void OnMount()
    {
    }

    public override void OnUnmount()
    {
    }
}
