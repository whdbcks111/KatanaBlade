using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFire : StatusEffect
{
    public static readonly float FireTicks = 0.3f;
    private float _timer = 0f;
    private GameObject _particle;

    public EffectFire(int level, float duration, Entity caster = null) 
        : base(level, duration, Resources.Load<Sprite>("Item/Icon/Spell/spell_24"), caster)
    {
    }

    public override void OnStart(Entity target)
    {
        _particle = EffectManager.EffectOneShot("Fire", target.transform.position);
    }

    public override void OnUpdate(Entity target)
    {
        if((_timer -= Time.deltaTime) <= 0f)
        {
            _timer += FireTicks;
            target.Damage(Level / FireTicks);
            _particle.transform.position = target.transform.position;
            if (target.HP <= 0 && _particle != null)
                Object.Destroy(_particle);
        }
    }

    public override void OnFinish(Entity target)
    {
        if(_particle != null)
            Object.Destroy(_particle);
    }
}
