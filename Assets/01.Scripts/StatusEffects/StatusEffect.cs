using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public int Level;
    public float Duration;
    public float MaxDuration { get; private set; }

    public StatusEffect(int level, float duration)
    {
        Level = level;
        MaxDuration = Duration = duration;
    }

    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract void OnFinish();
}
