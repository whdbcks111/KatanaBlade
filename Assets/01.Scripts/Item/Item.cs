using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Item
{
    public ItemType Type { get; protected set; }
    public string Name { get; protected set; }
    public int Count = 1;

    public Item(ItemType type, string name, int count)
    {
        Type = type;
        Name = name;
        Count = count;
    }

    public abstract void OnActiveUse();
    public abstract void PassiveUpdate();
}

public enum ItemType
{
    Accessory, Essence
}
