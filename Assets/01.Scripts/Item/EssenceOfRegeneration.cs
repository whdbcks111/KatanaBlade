using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfRegenerate : Item
{
    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;

    public EssenceOfRegenerate() 
        : base(ItemType.Essence, "????? ????", 
            string.Format(
                "??? ?? : HP?? <color=green>10</color> ???????. <color=gray>(???? ???? : {0:0.0}??)</color>\n" +
                "?? ???? ??? : 1??? HP?? <color=green>2</color> ???????.", Cooldown), 
            Resources.Load<Sprite>("Item/Icon/Essence/Essence_4"), count)
    {
    }

    public override void OnActiveUse()
    {
        if (_lastUsed > 0 && (Time.realtimeSinceStartup - _lastUsed) < Cooldown) return;
        _lastUsed = Time.realtimeSinceStartup;

        Player.Instance.Heal(10);
    }

    public override void PassiveUpdate()
    {
        Player.Instance.Heal(Time.deltaTime * 2);
    }
}
