using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfSwift : Item
{
    private static readonly float Cooldown = 5f;
    public EssenceOfSwift()
: base(ItemType.Essence, "신속의 정수",
    string.Format(
        "��� �� : �ֺ� ����ü�� <color=#404>�ı�</color>�ϴ� ������ �����մϴ�. <color=#aaa>(���� ��ñⰣ : {0:0.0}��)</color>\n", Cooldown),

    Resources.Load<Sprite>("Item/Icon/Essence/Essence_3"))
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
        Player.Instance.Stat.Multiply(StatType.MoveSpeed, 1.5f);
    }
}
