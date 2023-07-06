using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static Player Instance { get; private set; }

    public readonly Inventory Inventory = new();

    protected override void Awake()
    {
        base.Awake();
        Instance = this;

        Inventory.AddItem(new EssenceOfRegenerate(3));
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
