using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealArea : Interactable
{
    public float HealAmount;
    [Range(0.5f, 5f)]
    public float Radius;
    private bool _isHealed = false;


    private void Start()
    {
        //health = maxHP;
    }

    private void Update()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, Radius);
        foreach (var player in objects)
        {
            if (player.TryGetComponent(out Player p) && _isHealed == false)
            {
                p.Heal(HealAmount);
                print("Healed! " + HealAmount);
                _isHealed = true;
            }
        }
    }

    public override void OnInteract(Player player)
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
