using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingAx : Item
{

    public VikingAx() 
        : base(ItemType.Accessory, "����ŷ�� ����", 
            string.Format(
                "������ ���׿��� ����ŷ ��X������ �ֿ��ϴ� ������ ���ݷ��� �÷��ش�." +
                "\n<color=#0ff>���ݷ� 25% ����</color>"), 
            Resources.Load<Sprite>("Item/Icon/Accessory/axe"))
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
