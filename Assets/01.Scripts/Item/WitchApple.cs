using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchApple : Item
{

    public WitchApple() 
        : base(ItemType.Accessory, "마녀의 사과", 
            string.Format(
                "공주를 암살하기 위해 만든 사과로 보스에게 큰 피해를 줄 수 있다." +
                "\n<color=#0ff>보스데미지 30% 증가</ color>"), 
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
