using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfRegenerate : Item
{
    public EssenceOfRegenerate()
        : base(ItemType.Essence, "재생의 정수")
    {
    }

    public override void OnAcceptItem()
    {
        Debug.Log("재생의 정수 획득");
    }

    public override void OnActiveUse()
    {
        Debug.Log("재생의 정수 사용");
        //Player.Instance.Hp += 10;
    }

    public override void PassiveUpdate()
    {
        //Player.Instance.Hp += Time.deltaTime * 2;
    }
}