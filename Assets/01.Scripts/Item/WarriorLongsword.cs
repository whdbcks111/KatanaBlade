using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorLongsword : Item
{

    public WarriorLongsword() 
        : base(ItemType.Accessory, "용사의 장검", 
            string.Format(
                "용사가 마왕을 잡았던 장검으로 착용 시 용사와 같은 힘을 이끌어 낼 수있다." +
                "\n<color=#0ff>공격력 50% 증가</ color>"), 
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
