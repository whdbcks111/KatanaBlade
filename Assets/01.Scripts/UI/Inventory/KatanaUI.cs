using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KatanaUI : MonoBehaviour
{
    public float time;
    public Image image;

    void Start()
    {
        //FadeIn(time);
    }

    void Update()
    {
        var essence = Player.Instance.Inventory.MountedEssence;
        Color essenceColor = essence is null ? Color.white : ExtraMath.GetMainColor(essence.Icon);


        image.color = Color.Lerp(Color.white, essenceColor, Mathf.Clamp01(Player.Instance.EssenceCooldownRatio));
        
    }
}
