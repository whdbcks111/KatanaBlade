using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public int Level;
    public float Duration;
    public Entity Caster { get; private set; }
    public float MaxDuration { get; private set; }

    public StatusEffect(int level, float duration, Entity caster = null)
    {
        Level = level;
        MaxDuration = Duration = duration;
        Caster = caster;
    }

    public abstract void OnStart(Entity target);
    public abstract void OnUpdate(Entity target);
    public abstract void OnFinish(Entity target);
}
