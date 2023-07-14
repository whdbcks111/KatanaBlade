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
                "사용 시 : HP를 <color=green>10</color> 회복합니다. <color=#aaa>(재사용 대시기간 : {0:0.0}초)</color>\n" +
                "기본 지속 효과 : 1초당 HP를 <color=green>2</color> 회복합니다.", Cooldown),
            Resources.Load<Sprite>("Item/Icon/Essence/Essence_5"))

    {
    }

    public override void OnActiveUse()
    {
        Player.Instance.SetEssenceCooldown(Cooldown);
        Player.Instance.Heal(10);
        ParticleManager.Instance.SpawnParticle(Player.Instance.transform.position, "Healing");
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

