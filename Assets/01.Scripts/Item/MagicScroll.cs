using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicScroll : Item
{

    public MagicScroll() 
        : base(ItemType.Accessory, "�������� ����", 
            string.Format(
                "������ ���� ���� ���������ִ� �ű��� ��ũ���̴�." +
                "\n<color=#0ff>���� ������ 30%</ color>"), 
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
    }
}
