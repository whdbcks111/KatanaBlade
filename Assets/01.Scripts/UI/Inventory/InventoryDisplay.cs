using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public Image EssenceUI, AccessoryUI;
    public Image[] SlotList;

    private void Start()
    {
        for (int i = 0; i < Inventory.InventorySpace; i++)
        {
            SlotList[i].GetComponent<ItemIconUI>().ItemIndex = i;
        }
    }

    private void Update()
    {
        EssenceUI.sprite = Player.Instance.Inventory.MountedEssence?.Icon;
        AccessoryUI.sprite = Player.Instance.Inventory.MountedAccessory?.Icon;

        for (int i = 0; i < Inventory.InventorySpace; i++)
        {
            var item = Player.Instance.Inventory.GetItem(i);
            if (item is null) SlotList[i].sprite = null;
            else SlotList[i].sprite = item.Icon;

        }
    }

}


