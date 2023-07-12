using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryTest : Item
{

    public AccessoryTest() 
        : base(ItemType.Accessory, "테스트 장신구", 
            string.Format(
                "엑세사리 테스트"), 
            Resources.Load<Sprite>("Item/Icon/Essence/Essence_9"))
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
        Player.Instance.Stat.Multiply(StatType.ParryingAttackForce, 1.5f);
    }
}
