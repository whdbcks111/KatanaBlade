using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EssenceIconUI : MonoBehaviour, IPointerClickHandler
{

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
}
