
using static UnityEditor.Progress;

public class Inventory
{
    public static readonly int InventorySpace = 20;

    public Item MountedAccessory { get; private set; }
    public Item MountedEssence { get; private set; }

    private readonly Item[] _contents = new Item[InventorySpace];

    public bool AddItem(Item item)
    {
        for (var i = 0; i < _contents.Length; i++)
        {
            if (_contents[i] is null)
            {
                _contents[i] = item;
                return true;
            }
            else if (_contents[i].GetType() == item.GetType())
            {
                return false;
            }
        }
        return false;
    }

    public Item GetItem(int idx)
    {
        return _contents[idx];
    }
    public void RemoveItem(int index, int count)
    {
        _contents[index] = null;
    }

    public void SwapSlot(int index1, int index2)
    {
        (_contents[index1], _contents[index2]) = (_contents[index2], _contents[index1]);
    }

    public void UnmountAccessory()
    {
        MountedAccessory.OnUnmount();
        AddItem(MountedAccessory);
        MountedAccessory = null;
    }

    public void UnmountEssence()
    {
        MountedEssence.OnUnmount();
        AddItem(MountedEssence);
        MountedEssence = null;
    }

    public void MountItem(int index)
    {
        var item = _contents[index];
        if (item is null) return;

        switch(item.Type)
        {
            case ItemType.Accessory:
                if (MountedAccessory is not null) AddItem(MountedAccessory);
                MountedAccessory = item;
                break;
            case ItemType.Essence:
                if (MountedEssence is not null) AddItem(MountedEssence);
                MountedEssence = item;
                break;
        }

        item.OnMount();

        _contents[index] = null;
    }
}
