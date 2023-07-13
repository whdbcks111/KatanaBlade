using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneGlove : Item
{

    public RuneGlove() 
        : base(ItemType.Accessory, "룬 글러브", 
            string.Format(
                "신비한 룬의 힘이 담긴 글러브로 패링에 소모되는 비용이 줄어든다." +
                "\n<color=#0ff>패링 비용 30% 감소</ color>"), 
            Resources.Load<Sprite>("Item/Icon/Accessory/gloves"))
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
        Player.Instance.Stat.Multiply(StatType.ParryingCost, 0.7f);
    }
}
