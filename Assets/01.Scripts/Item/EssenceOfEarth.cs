using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfEarth : Item
{


    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;
    private static readonly float PassiveTick = 5f;
    private static readonly float ActiveMag = 5f;
    private static readonly float SkillWidth = 7f;
    private float _dT;

    public EssenceOfEarth()
    : base(ItemType.Essence, "대지의 정수",
        string.Format(
            "사용 시 : 주변 적들을 <color=brown>띄웁</color>니다. <color=gray>(재사용 대시기간 : {0:0.0}초)</color>\n" +
            "기본 지속 효과 : {1}초마다 주변 적을 <color=brown>기절</color>시킵니다.", Cooldown, PassiveTick),
        Resources.Load<Sprite>("Item/Icon/EssenceOfRegeneration"))
    {
    }

    [ContextMenu("액티브 사용")]
    public override void OnActiveUse()
    {
        if (_lastUsed > 0 && (Time.realtimeSinceStartup - _lastUsed) < Cooldown) return;
        _lastUsed = Time.realtimeSinceStartup;

        Collider2D[] area = Physics2D.OverlapBoxAll(Player.Instance.transform.position, new Vector2(SkillWidth * 2, 2f), 0f);
        foreach (var enemy in area)
        {
            if(enemy.GetComponent<Entity>() is Monster)
            {
                //대상과의 거리에 따라 에어본이 달라짐
                enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (ActiveMag + (SkillWidth - Vector2.Distance(enemy.transform.position, Player.Instance.transform.position))), ForceMode2D.Impulse);
            }
        }
    }

    public override void PassiveUpdate()
    {
        _dT += Time.deltaTime;
        if(_dT > PassiveTick)
        {
            _dT = 0;
        }
    }
}
