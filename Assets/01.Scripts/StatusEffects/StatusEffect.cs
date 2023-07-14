using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public Sprite Icon;
    public int Level;
    public float Duration;
    public Entity Caster { get; private set; }
    public float MaxDuration { get; private set; }

    public StatusEffect(int level, float duration, Sprite icon, Entity caster = null)
    {
        Level = level;
        MaxDuration = Duration = duration;
        Caster = caster;
        Icon = icon;
    }

    public abstract void OnStart(Entity target);
    public abstract void OnUpdate(Entity target);
    public abstract void OnFinish(Entity target);
}
