using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFire : StatusEffect
{
    public static readonly float FireTicks = 0.3f;
    private float _timer = 0f;

    public EffectFire(int level, float duration, Entity caster = null) : base(level, duration, caster)
    {
    }

    public override void OnStart(Entity target)
    {
    }

    public override void OnUpdate(Entity target)
    {
        if((_timer -= Time.deltaTime) <= 0f)
        {
            _timer += FireTicks;
            target.Damage(Level / FireTicks);
        }
    }

    public override void OnFinish(Entity target)
    {
    }
}
