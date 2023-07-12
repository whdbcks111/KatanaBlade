using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingAx : Item
{

    public VikingAx() 
        : base(ItemType.Accessory, "바이킹의 도끼", 
            string.Format(
                "옆동네 리그에서 바이킹 올X프씨가 애용하던 도끼로 공격력을 올려준다." +
                "\n<color=#0ff>공격력 25% 증가</color>"), 
            Resources.Load<Sprite>("Item/Icon/Accessory/axe"))
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
