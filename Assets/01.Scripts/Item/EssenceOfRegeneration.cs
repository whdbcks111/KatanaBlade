using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfRegeneration : Item
{
    private static readonly float Cooldown = 5f;
    private GameObject _passiveEffect;
    private float _dT = 0f;

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
        GameObject particle = EffectManager.EffectOneShot("Healing Particle", Player.Instance.transform.position + Vector3.up);
        particle.transform.localScale = Vector3.one * 2f;
    }

    public override void PassiveUpdate()
    {
        Player.Instance.Heal(Time.deltaTime * 2);

        _dT += Time.deltaTime;
        if(_dT > 2f)
        {
            _passiveEffect.transform.position = Player.Instance.transform.position;
            _passiveEffect.GetComponent<ParticleSystem>().Play();
            _dT = 0f;
        }
    }

    public override void OnMount()
    {
        _passiveEffect = EffectManager.EffectOneShot("Healing Particle", Player.Instance.transform.position);
        _passiveEffect.transform.localScale = Vector3.one * .3f;
    }

    public override void OnUnmount()
    {
    }
}

