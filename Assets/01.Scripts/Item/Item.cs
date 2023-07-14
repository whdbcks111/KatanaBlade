using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Item : ScriptableObject
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

    public abstract void OnMount();
    public abstract void OnUnmount();
    public abstract void OnActiveUse();
    public abstract void PassiveUpdate();

    public static bool operator >(Item item1, Item item2)
    {
        if (item2 is null) return true;
        if (item1 is null) return false;

        if(item1.Type == item2.Type)
        {
            return string.Compare(item1.Name, item2.Name) < 0;
        }
        else
        {
            return item1.Type == ItemType.Essence;
        }
    }
    public static bool operator <(Item item1, Item item2)
    {
        return !(item1 > item2) && item1 != item2;
    }
}

public enum ItemType
{
    Accessory, Essence
}
