using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int ItemIndex;
    public GameObject itemPopup;
    [HideInInspector]
    public Sprite itemIcon;
    [HideInInspector]
    public string itemDescription;
    [HideInInspector]
    public string itemName;
    [HideInInspector]
    public string itemAbility;

    [HideInInspector]
    public bool canBuy { get; private set; }
    private bool showInfo;
    private bool onCursor;
    private Coroutine showInfoCor;


    private void Update()
    {
        print(showInfo);
        if (!onCursor)
            return;
        if (showInfo)
        {
            //var item = Player.Instance.Inventory.GetItem(ItemIndex);
            //if (item is not null)
            GameManager.instance.ShopItemPopup(GameManager.instance.ShopPopup, itemIcon, itemName, itemDescription);

        }
        else
        {
            GameManager.instance.HideShopPopup(GameManager.instance.ShopPopup);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onCursor = true;
        if (showInfoCor == null)
        {
            showInfoCor = StartCoroutine(Timer(1f));
        }

    }


    public void OnPointerExit(PointerEventData eventData)
    {
        onCursor = false;
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

    public void SetCanBuy(bool b) => canBuy = b;
}