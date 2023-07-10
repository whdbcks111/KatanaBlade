using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Item
{
    public ItemType Type { get; protected set; }
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public Sprite Icon { get; protected set; }

    protected Item(ItemType type, string name, string description, Sprite icon)
    {
        Type = type;
        Name = name;
        Description = description;
        Icon = icon;
    }

    public abstract void OnActiveUse();
    public abstract void PassiveUpdate();
}

public enum ItemType
{
    Accessory, Essence
}
