using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FlyingProjectile : Projectile
{
    public Tilemap tilemap;
    public Transform target;

    public void Update()
    {
        if (target != null && tilemap != null)
            Pathfinder.Follow(tilemap, transform, target.position, Speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Entity entity)) return;
        
        entity.Damage(10f);
        Destroy(this.gameObject, 0f);
            
        ParticleManager.Instance.SpawnParticle(transform.position, "Smoke");
    }
}
