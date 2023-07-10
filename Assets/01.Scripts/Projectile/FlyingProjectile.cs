using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FlyingProjectile : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform target;

    public void Update()
    {
        var position = transform.position;
        var nextPos = Pathfinder.GetNextPath(tilemap, position, target.position);
        position = Vector2.MoveTowards(position, nextPos, Time.deltaTime * 6);
        transform.position = position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Entity entity)) return;
        
        entity.Damage(10f);
        Destroy(this.gameObject, 0f);
            
        ParticleManager.Instance.SpawnParticle(transform.position, "Smoke");
    }
}
