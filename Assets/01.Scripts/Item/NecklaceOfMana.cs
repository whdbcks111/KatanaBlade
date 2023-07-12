using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecklaceOfMana : Item
{

    public NecklaceOfMana() 
        : base(ItemType.Accessory, "여행자의 부츠", 
            string.Format(
                "마나 친밀도가 높은 목걸이로 정수의 힘을 빨리 이끌어낼 수 있다." +
                "\n<color=#0ff>정수 쿨타임 30% 감소</ color>"), 
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
    }
}
