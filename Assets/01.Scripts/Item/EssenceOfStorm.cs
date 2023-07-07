using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfStorm : Item
{
    public EssenceOfStorm()
    : base(ItemType.Essence, "Àç»ýÀÇ Á¤¼ö")
    {
    }
    public override void OnAcceptItem()
    {
        Debug.Log("ÆøÇ³ÀÇ Á¤¼ö È¹µæ");
    }

    public override void OnActiveUse()
    {
        Debug.Log("ÆøÇ³ÀÇ Á¤¼ö »ç¿ëÇÔ");
    }

    public override void PassiveUpdate()
    {
        Debug.Log("ÆøÇ³ÀÇ Á¤¼ö ÆÐ½Ãºê");
    }
}
