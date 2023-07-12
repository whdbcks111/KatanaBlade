using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopDisplay : MonoBehaviour
{
    public int ItemIndex;
    private bool showInfo;
    private Coroutine showInfoCor;

    private void Update()
    {
        print(showInfo);
        if (showInfo)
        {
            var item = Player.Instance.Inventory.GetItem(ItemIndex);
            if (item is not null)
                GameManager.instance.ShopItemPopup(this, item.Icon, item.Name, item.Description);
        }
        else
        {
            GameManager.instance.HideShopPopup(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (showInfoCor == null)
        {
            showInfoCor = StartCoroutine(Timer(1f));
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(showInfoCor);
        showInfoCor = null;
        showInfo = false;
    }

    private IEnumerator Timer(float time)
    {
        showInfo = false;
        yield return new WaitForSeconds(time);
        showInfo = true;
    }
}