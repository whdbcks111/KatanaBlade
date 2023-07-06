using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemIconUI : MonoBehaviour, IPointerClickHandler
{
    public int ItemIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            var popup = GameManager.instance.CreateEquipPopup();
            popup.EquipAction = () =>
            {
                print(ItemIndex);
                Player.Instance.Inventory.MountItem(ItemIndex);
                popup.Clear();
            };
        }
    }
}
