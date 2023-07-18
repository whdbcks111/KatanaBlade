using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MouseColorText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI _text;
    private Color _originColor;
    [SerializeField] private Color _color;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _originColor = _text.color;
        _text.color = _color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.color = _originColor;
    }

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _originColor = _text.color;
    }
}
