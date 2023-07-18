
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
                Sort();
                return true;
            }
            else if (_contents[i].GetType() == item.GetType())
            {
                return false;
            }
        }
        return false;
    }

    public void Sort()
    {
        for(int i = 0; i < _contents.Length; i++)
        {
            int maxIdx = i;
            for(int j = i + 1; j < _contents.Length; j++)
            {
                if (_contents[maxIdx] < _contents[j])
                    maxIdx = j;
            }
            (_contents[i], _contents[maxIdx]) = (_contents[maxIdx], _contents[i]);
        }
    }

    public Item GetItem(int idx)
    {
        return _contents[idx];
    }
    public void RemoveItem(int index)
    {
        _contents[index] = null;
        Sort();
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
        RemoveItem(index);

        switch (item.Type)
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
    }
}
