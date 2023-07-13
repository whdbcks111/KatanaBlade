using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyCloak : Item
{

    public FairyCloak() 
        : base(ItemType.Accessory, "요정의 망토", 
            string.Format(
                "바람의 요정의 힘이 당긴 망토로 대쉬쿨타임이 줄어든다." +
                "\n<color=#0ff>대쉬 쿨타임 30% 감소</ color>"), 
            Resources.Load<Sprite>("Item/Icon/Accessory/cloaks"))
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
