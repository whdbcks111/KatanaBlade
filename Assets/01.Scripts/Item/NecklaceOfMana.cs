using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecklaceOfMana : Item
{

    public NecklaceOfMana() 
        : base(ItemType.Accessory, "���� �����", 
            string.Format(
                "���� ģ�е��� ���� ����̷� ������ ���� ���� �̲�� �� �ִ�." +
                "\n<color=#0ff>���� ��Ÿ�� 30% ����</ color>"), 
            Resources.Load<Sprite>("Item/Icon/Accessory/necklace"))
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
        Player.Instance.Stat.Multiply(StatType.EssenceCooldown, 0.7f);
    }
}
