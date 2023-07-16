using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfSwift : Item
{
    private static readonly float Cooldown = 5f;
    public EssenceOfSwift()
: base(ItemType.Essence, "신속의 정수",
    string.Format(
        "사용 시 : <color=#ff72f5ff>돌풍</color>을 일으켜 돌진합니다. 그 후 커서 방향으로 세 갈래의 투사체를 발사합니다. <color=#aaa>(재사용 대시기간 : {0:0.0}초)</color>\n" +
                "기본 지속 효과 : 이동속도가 25% 증가합니다.", Cooldown),

    Resources.Load<Sprite>("Item/Icon/Essence/Essence_11"))
    {

    }
    public override void OnActiveUse()
    {
        Player.Instance.SetEssenceCooldown(Cooldown);

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float stare = (Player.Instance.transform.position.x > pos.x ? -1 : 1);

        Vector2 movePos = Vector2.zero;

        var hit = Physics2D.BoxCast(Player.Instance.transform.position,
            Player.Instance.GetComponent<Collider2D>().bounds.size * .9f,
            0,
            Vector2.right * stare,
                Player.Instance.Stat.Get(StatType.DashLength), LayerMask.GetMask("Platform"));

        if (hit)
        {
            movePos = new Vector2(hit.point.x - stare * Player.Instance.GetComponent<Collider2D>().bounds.size.x / 2, Player.Instance.transform.position.y);
        }
        else
        {
            movePos = new Vector2(Player.Instance.transform.position.x + Player.Instance.Stat.Get(StatType.DashLength) * stare, Player.Instance.transform.position.y);
        }
        Player.Instance.StartCoroutine(SkillCor(movePos));
    }

    public override void OnMount()
    {
    }

    public override void OnUnmount()
    {
    }

    public override void PassiveUpdate()
    {
        Player.Instance.Stat.Multiply(StatType.MoveSpeed, 1.25f);
    }

    private IEnumerator SkillCor(Vector2 moveTo)
    {
        float dT = 0;
        Vector2 origin = Player.Instance.transform.position;
        while (dT < 0.2f)
        {
            Player.Instance.transform.position = Vector2.Lerp(origin, moveTo, dT / 0.2f);
            yield return null;
            dT += Time.deltaTime;
        }

        for (int i = 0; i < 3; i++)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Projectile projectile = Instantiate(Resources.Load<GameObject>("Item/SwiftProjectile"), Player.Instance.transform.position, Quaternion.identity).GetComponent<Projectile>();
            projectile.SetOwner(Player.Instance, ExtraMath.DirectionToAngle((pos - (Vector2)Player.Instance.transform.position).normalized) + (5 * (i - 1)));
        }
    }
}
