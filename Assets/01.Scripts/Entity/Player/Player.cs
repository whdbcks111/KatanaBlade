using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static Player Instance { get; private set; }

    public readonly Inventory Inventory = new();

    private PlayerController _controller;
    private Animator _animator;
    protected override void Awake()
    {
        base.Awake();
        Instance = this;


        Inventory.AddItem(new EssenceOfRegeneration());
        Inventory.AddItem(new AccessoryTest());

        _controller = GetComponent<PlayerController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
    public override void Damage(float damageAmount)
    {
        HP -= damageAmount;

        if (_controller.IsConscious)
            _animator.SetTrigger("Hit");
        
        if (HP <= 0)
        {
            StopAllCoroutines();
            _controller.IsConscious = false;
            _animator.SetBool("Dead", true);
        }
    }
}
