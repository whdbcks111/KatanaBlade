using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfSwift : Item
{
    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;
    public EssenceOfSwift()
: base(ItemType.Essence, "공허의 정수",
    string.Format(
        "사용 시 : 주변 투사체를 <color=darkviolet>파괴</color>하는 영역을 생성합니다. <color=gray>(재사용 대시기간 : {0:0.0}초)</color>\n", Cooldown),
    Resources.Load<Sprite>("Item/Icon/Essence/Essence_3"))
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
