using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOfTheBasilisk : Item
{

    public BloodOfTheBasilisk() 
        : base(ItemType.Accessory, "바실리스크의 피", 
            string.Format(
                "괴수 바실리스크의 피로 최대체력을 증가시켜준다." +
                "\n<color=#0ff>HP 30% 증가</ color>"), 
            Resources.Load<Sprite>("Item/Icon/Accessory/hp"))
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
