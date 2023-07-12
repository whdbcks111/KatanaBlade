using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfStorm : Item
{
    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;

    public EssenceOfStorm()
: base(ItemType.Essence, "폭풍의 정수",
    string.Format(
        "사용 시 : 주변 적들을 <color=brown>띄웁</color>니다. <color=gray>(재사용 대시기간 : {0:0.0}초)</color>\n" +
        "기본 지속 효과 : {1}초마다 주변 적을 <color=brown>기절</color>시킵니다.", Cooldown),
    Resources.Load<Sprite>("Item/Icon/Essence/Essence_0"))
    {
    }

    public override void OnActiveUse()
    {
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
}
