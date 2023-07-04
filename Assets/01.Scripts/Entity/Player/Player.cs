using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static Player Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
