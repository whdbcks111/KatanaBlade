using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfRegenerate : Item
{
    private float _lastUsed = -1;
    private float _cooldown = 5f;

    public EssenceOfRegenerate(int count) 
        : base(ItemType.Essence, "재생의 정수", 
            "사용 시 : HP를 10 회복합니다. (재사용 대시기간 : 5초)\n" +
            "기본 지속 효과 : 1초당 HP를 2 회복합니다.", 
            Resources.Load<Sprite>("Item/Icon/EssenceOfRegeneration"), count)
    {
    }

    public override void OnActiveUse()
    {
        if (_lastUsed > 0 && (Time.realtimeSinceStartup - _lastUsed) < _cooldown) return;
        _lastUsed = Time.realtimeSinceStartup;

        Player.Instance.Heal(10);
    }

    public override void PassiveUpdate()
    {
        Player.Instance.Heal(Time.deltaTime * 2);
    }
}
