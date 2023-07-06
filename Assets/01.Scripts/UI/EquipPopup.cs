using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EquipPopup : MonoBehaviour
{
    public Button EquipButton;
    public Action EquipAction;

    public void Clear()
    {
        Destroy(gameObject);
    }

    public void Equip()
    {
        EquipAction();
    }
}
