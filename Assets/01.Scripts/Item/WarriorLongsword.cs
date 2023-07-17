using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorLongsword : Item
{

    public WarriorLongsword() 
        : base(ItemType.Accessory, "����� ���", 
            string.Format(
                "��簡 ������ ��Ҵ� ������� ���� �� ���� ���� ���� �̲��� �� ���ִ�." +
                "\n<color=#0ff>���ݷ� 50% ����</ color>"), 
            Resources.Load<Sprite>("Item/Icon/Accessory/sword"))
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
