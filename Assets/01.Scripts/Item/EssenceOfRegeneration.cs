using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfRegenerate : Item
{
    private float _lastUsed = -1;
    private float _cooldown = 5f;

    public EssenceOfRegenerate(int count) 
        : base(ItemType.Essence, "재생의 정수", count)
    {
    }

    public override void OnActiveUse()
    {
        if (_lastUsed > 0 && (Time.realtimeSinceStartup - _lastUsed) < _cooldown) return;
        _lastUsed = Time.realtimeSinceStartup;

        //Player.Instance.Hp += 10;
    }

    public override void PassiveUpdate()
    {
        //Player.Instance.Hp += Time.deltaTime * 2;
    }
}
