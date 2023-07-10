using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Decalcomani : MonoBehaviour
{
    public Image Image;
    private Image _self;

    void Start()
    {
        _self = GetComponent<Image>();
    }

    void Update()
    {
        _self.sprite = Image.sprite;
    }
}
