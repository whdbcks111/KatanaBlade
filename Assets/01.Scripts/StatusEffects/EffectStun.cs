using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectStun : StatusEffect
{
    public EffectStun(int level, float duration, Entity caster = null) : base(level, duration, caster)
    {
    }

    public override void OnStart(Entity target)
    {
    }

    public override void OnUpdate(Entity target)
    {
    }

    public override void OnFinish(Entity target)
    {
    }
}
