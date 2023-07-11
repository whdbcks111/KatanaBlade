using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopupClose : MonoBehaviour
{
    public Button _OnClickCancelButton;
    public GameObject shopPanel;

    public void ActiveButton()
    {
        shopPanel.SetActive(false);
    }
}