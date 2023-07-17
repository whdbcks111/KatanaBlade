using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinCoin : Item
{
    private int Price;

    public GoblinCoin() 
        : base(ItemType.Accessory, "����� ����", 
            string.Format(
                "������� ��Ƶ� �������� �繰���� �÷��ش�." +
                "\n<color=#0ff>���ȹ�淮 30% ����</ color>"), 
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
