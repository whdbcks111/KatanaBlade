using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfRegenerate : Item
{
    public EssenceOfRegenerate()
        : base(ItemType.Essence, "����� ����")
    {
    }

    public override void OnAcceptItem()
    {
        Debug.Log("����� ���� ȹ��");
    }

    public override void OnActiveUse()
    {
        Debug.Log("����� ���� ���");
        //Player.Instance.Hp += 10;
    }

    public override void PassiveUpdate()
    {
        //Player.Instance.Hp += Time.deltaTime * 2;
    }
}