using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinCoin : Item
{
    private int Price;

    public GoblinCoin() 
        : base(ItemType.Accessory, "고블린의 코인", 
            string.Format(
                "고블린들이 모아둔 코인으로 재물운을 늘려준다." +
                "\n<color=#0ff>골드획득량 30% 증가</ color>"), 
            Resources.Load<Sprite>("Item/Icon/Accessory/coins"))
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
        Player.Instance.Stat.Multiply(StatType.GoldObtainMultiplier, 1.3f);
    }
}
