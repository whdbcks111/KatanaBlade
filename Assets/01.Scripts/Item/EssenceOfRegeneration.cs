using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfRegeneration : Item
{
    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;

    public EssenceOfRegeneration()
        : base(ItemType.Essence, "재생의 정수",
            string.Format(
                "사용 시 : HP를 <color=green>10</color> 회복합니다. <color=gray>(재사용 대시기간 : {0:0.0}초)</color>\n" +
                "기본 지속 효과 : 1초당 HP를 <color=green>2</color> 회복합니다.", Cooldown),
            Resources.Load<Sprite>("Item/Icon/Essence/Essence_5"))

    {
    }

    public override void OnActiveUse()
    {
        if (_lastUsed > 0 && (Time.realtimeSinceStartup - _lastUsed) < Cooldown) return;
        _lastUsed = Time.realtimeSinceStartup;

        Player.Instance.Heal(10);
        Player.Instance.SetEssenceCooldown(5);
    }

    public override void PassiveUpdate()
    {
        Player.Instance.Heal(Time.deltaTime * 2);
    }

    public override void OnMount()
    {
    }

    public override void OnUnmount()
    {
    }
}

