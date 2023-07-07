using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Item : MonoBehaviour
{
    public ItemType Type { get; protected set; }
    public string Name { get; protected set; }

    public Sprite ItemSprite;
    public float Cooldown;

    public Item(ItemType type, string name)
    {
        Type = type;
        Name = name;
    }

    public abstract void OnAcceptItem();
    public abstract void OnActiveUse();
    public abstract void PassiveUpdate();

}

public enum ItemType
{
    Accessory, Essence
}
