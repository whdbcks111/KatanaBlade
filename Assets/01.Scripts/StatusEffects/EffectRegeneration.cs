using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRegeneration : StatusEffect
{
    public EffectRegeneration(int level, float duration, Entity caster = null) 
        : base(level, duration, Resources.Load<Sprite>("Item/Icon/Spell/spell_40"), caster)
    {
    }

    public override void OnStart(Entity target)
    {
    }

    public override void OnUpdate(Entity target)
    {
        target.Heal(Level * Time.deltaTime);
    }

    public override void OnFinish(Entity target)
    {
    }
}
