using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Util;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private static ParticleManager _instance = null;

    public static ParticleManager Instance => _instance;

    public GameObject particlePrefebs;
    public Transform spawnPoint;
    

    private void Awake()
    {
        
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SpawnParticle(Vector3 position, string particleName)
    {
        var system = Instantiate(Resources.Load<ParticleSystem>("Particle/" + particleName), 
            position, Quaternion.identity);
        system.AddComponent<ParticleDestroy>();
    }
    
}
