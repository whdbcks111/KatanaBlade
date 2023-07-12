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
        player.Teleport(BossMapPos);
        StartCoroutine(ReturnPortalRoutine());
        player.StartCoroutine(ScreenEffectRoutine());
    }

    private IEnumerator ScreenEffectRoutine()
    {
        for (var i = 0f; i < 1f; i += Time.deltaTime)
        {
            yield return null;
            ScreenEffectManager.Instance.SetSaturation(Mathf.Clamp01(1 - i) * 100 - 100);
        }
        for (var i = 0f; i < 1f; i += Time.deltaTime)
        {
            yield return null;
            ScreenEffectManager.Instance.SetSaturation(Mathf.Clamp01(i) * 100 - 100);
        }
        ScreenEffectManager.Instance.ResetSaturation();
    }

    private IEnumerator ReturnPortalRoutine()
    {
        Vector3 lastPos = _boss.transform.position;
        while(!_boss.Equals(null))
        {
            lastPos = _boss.transform.position;
            yield return null;
        }
        var p = Instantiate(Resources.Load<ReturnPortal>("Interactable/ReturnPortal"), lastPos, Quaternion.identity);
        p.ParentPortal = this;
    }
}
