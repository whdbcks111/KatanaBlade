using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static GameObject EffectOneShot(string effectName, Vector3 pos, float effectTIme)
    {
        GameObject effect = Instantiate<GameObject>((Resources.Load("Particle/" + effectName) as GameObject), pos, Quaternion.identity);
        return effect;
    }
}
