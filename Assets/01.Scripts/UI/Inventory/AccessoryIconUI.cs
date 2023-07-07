using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AccessoryIconUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private bool showInfo;
    private Coroutine showInfoCor;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right &&
            Player.Instance.Inventory.MountedAccessory is not null)
        {
            var popup = GameManager.instance.CreateUnequipPopup();
            popup.UnEquipAction = () =>
            {
                Player.Instance.Inventory.UnmountAccessory();
                popup.Clear();
            };
        }
    }

    private void Update()
    {
        print("Acc update");
        if (showInfo)
        {
            var item = Player.Instance.Inventory.MountedAccessory;
            if (item is not null)
                GameManager.instance.ShowItemPopup(this, item.Icon, item.Name, item.Description);
        }
        else
        {
            GameManager.instance.HideItemPopup(this);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Acc");
        if (showInfoCor == null)
        {
            showInfoCor = StartCoroutine(Timer(1f));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showInfoCor is not null) StopCoroutine(showInfoCor);
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
