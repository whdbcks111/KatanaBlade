using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static readonly int InventorySpace = 20;

    public Item MountedAccessory { get; private set; }
    public Item MountedEssence { get; private set; }

    private readonly Item[] _contents = new Item[InventorySpace];

    public bool AddItem(Item item)
    {
        for(var i = 0; i < _contents.Length; i++)
        {
            if (_contents[i] is null) continue;
            if (_contents[i].GetType() == item.GetType())
            {
                _contents[i].Count += item.Count;
                return true;
            }
            else if (_contents[i] is null)
            {
                _contents[i] = item;
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(int index, int count)
    {
        if (_contents[index] is null) return;

        if ((_contents[index].Count -= count) <= 0)
        {
            _contents[index] = null;
        }
    }

    public void RemoveItem<T>(int count) where T : Item
    {
        for (var i = 0; i < _contents.Length; i++)
        {
            if (_contents[i] is T)
            {
                RemoveItem(i, count);
                return;
            }
        }
    }

    public void SwapSlot(int index1, int index2)
    {
        (_contents[index1], _contents[index2]) = (_contents[index2], _contents[index1]);
    }

    public void UnmountAccessory()
    {

        AddItem(MountedAccessory);
    }

    public void MountItem(int index)
    {
        var item = _contents[index];
        if (item is null) return;

        switch(item.Type)
        {
            case ItemType.Accessory:
                MountedAccessory = item;
                break;
            case ItemType.Essence:
                MountedEssence = item;
                break;
        }

        _contents[index] = null;
    }
}
