using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneGlove : Item
{

    public RuneGlove() 
        : base(ItemType.Accessory, "�� �۷���", 
            string.Format(
                "�ź��� ���� ���� ��� �۷���� �и��� �Ҹ�Ǵ� ����� �پ���." +
                "\n<color=#0ff>�и� ��� 30% ����</ color>"), 
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
