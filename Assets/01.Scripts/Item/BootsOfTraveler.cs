using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsOfTraveler : Item
{

    public BootsOfTraveler() 
        : base(ItemType.Accessory, "여행자의 부츠", 
            string.Format(
                "이름 모르는 여행가가 착용한 부츠로 이동속도를 증가시켜준다." +
                "\n<color=#0ff>이동속도 30% 증가</color>"), 
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
