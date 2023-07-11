using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPortal : Interactable
{
    public Vector3 BossMapPos;
    public Boss BossPrefab;

    private Boss _boss;

    public override void OnInteract(Player player)
    {
        _boss = Instantiate(BossPrefab, BossMapPos, Quaternion.identity);
        player.transform.position = BossMapPos;
        StartCoroutine(ReturnPortalRoutine());
    }

    private IEnumerator ReturnPortalRoutine()
    {
        yield return new WaitUntil(() => _boss.HP <= 0);
        var p = Instantiate(Resources.Load<ReturnPortal>("Interactable/ReturnPortal"), _boss.transform.position, Quaternion.identity);
        p.ParentPortal = this;
    }
}
