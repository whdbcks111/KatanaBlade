using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchApple : Item
{

    public WitchApple() 
        : base(ItemType.Accessory, "������ ���", 
            string.Format(
                "���ָ� �ϻ��ϱ� ���� ���� ����� �������� ū ���ظ� �� �� �ִ�." +
                "\n<color=#0ff>���������� 30% ����</ color>"), 
            Resources.Load<Sprite>("Item/Icon/Accessory/apple"))
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
        Player.Instance.Stat.Multiply(StatType.BossAttackForce, 1.3f);
    }
}
