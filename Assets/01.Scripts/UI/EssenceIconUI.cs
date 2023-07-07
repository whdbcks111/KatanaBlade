using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EssenceIconUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private bool showInfo;
    private Coroutine showInfoCor;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            var popup = GameManager.instance.CreateUnequipPopup();
            popup.UnEquipAction = () =>
            {
                Player.Instance.Inventory.UnmountEssence();
                popup.Clear();
            };
        }
    }
    private void Update()
    {
        print(showInfo);
        if (showInfo)
        {
            var item = Player.Instance.Inventory.MountedEssence;
            GameManager.instance.ShowItemPopup(this, item.Icon, item.Name, item.Description);
        }
        else
        {
            GameManager.instance.HideItemPopup(this);
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
            if(showInfoCor is not null) StopCoroutine(showInfoCor);
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
