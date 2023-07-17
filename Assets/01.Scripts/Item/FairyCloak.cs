using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyCloak : Item
{

    public FairyCloak() 
        : base(ItemType.Accessory, "������ ����", 
            string.Format(
                "�ٶ��� ������ ���� ��� ����� �뽬��Ÿ���� �پ���." +
                "\n<color=#0ff>�뽬 ��Ÿ�� 30% ����</ color>"), 
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
        Player.Instance.Stat.Multiply(StatType.DashCooldown, 0.7f);
    }
}
