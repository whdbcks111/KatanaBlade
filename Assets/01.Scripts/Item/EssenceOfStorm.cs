using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfStorm : Item
{
    public EssenceOfStorm()
    : base(ItemType.Essence, "����� ����")
    {
    }
    public override void OnAcceptItem()
    {
        Debug.Log("��ǳ�� ���� ȹ��");
    }

    public override void OnActiveUse()
    {
        Debug.Log("��ǳ�� ���� �����");
    }

    public override void PassiveUpdate()
    {
        Debug.Log("��ǳ�� ���� �нú�");
    }
}
