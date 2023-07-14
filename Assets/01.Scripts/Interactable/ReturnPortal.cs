using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPortal : Interactable
{
    public BossPortal ParentPortal;

    public override void OnInteract(Player player)
    {
        player.Teleport(ParentPortal.transform.position);
        Destroy(gameObject);
        Destroy(ParentPortal.gameObject);
    }
}
