using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static GameObject EffectOneShot(string effectName, Vector3 pos)
    {
        ParticleSystem load = Resources.Load<ParticleSystem>("Particle/" + effectName);
        ParticleSystem effect = Instantiate(load.gameObject, pos, Quaternion.identity).GetComponent<ParticleSystem>();
        var main = effect.main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;
        return effect.gameObject;
    }

    public static GameObject EffectLoop(string effectName, Vector3 pos)
    {
        ParticleSystem load = Resources.Load<ParticleSystem>("Particle/" + effectName);
        ParticleSystem effect = Instantiate(load.gameObject, pos, Quaternion.identity).GetComponent<ParticleSystem>();
        var main = effect.main;
        main.loop = true;
        return effect.gameObject;
    }
}
