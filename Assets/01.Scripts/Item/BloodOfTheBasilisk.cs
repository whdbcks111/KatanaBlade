using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOfTheBasilisk : Item
{

    public BloodOfTheBasilisk() 
        : base(ItemType.Accessory, "�ٽǸ���ũ�� ��", 
            string.Format(
                "���� �ٽǸ���ũ�� �Ƿ� �ִ�ü���� ���������ش�." +
                "\n<color=#0ff>HP 30% ����</ color>"), 
            Resources.Load<Sprite>("Item/Icon/Accessory/hp"))
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
        Player.Instance.Stat.Multiply(StatType.MaxHP, 1.3f);
    }
}
