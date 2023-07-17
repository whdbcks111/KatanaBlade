using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsOfTraveler : Item
{

    public BootsOfTraveler() 
        : base(ItemType.Accessory, "�������� ����", 
            string.Format(
                "�̸� �𸣴� ���డ�� ������ ������ �̵��ӵ��� ���������ش�." +
                "\n<color=#0ff>�̵��ӵ� 30% ����</color>"), 
            Resources.Load<Sprite>("Item/Icon/Accessory/boots"))
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
        Player.Instance.Stat.Multiply(StatType.MoveSpeed, 1.3f);
    }
}
