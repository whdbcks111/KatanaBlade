using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicScroll : Item
{

    public MagicScroll() 
        : base(ItemType.Accessory, "마법 스크롤", 
            string.Format(
                "정수의 힘을 더욱 증폭시켜주는 신기한 스크롤이다." +
                "\n<color=#0ff>정수 효과 + 50%</ color>"), 
            Resources.Load<Sprite>("Item/Icon/Accessory/scroll"))
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
        Player.Instance.Stat.Multiply(StatType.EssenceForce, 1.5f);
    }
}
