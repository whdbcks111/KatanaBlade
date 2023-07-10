using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnequipPopup : MonoBehaviour
{
    public Button UnEquipButton;
    public Action UnEquipAction;

    public void Clear()
    {
        GameManager.instance.ClosePopup();
    }

    public void UnEquip()
    {
        UnEquipAction();
    }
}
