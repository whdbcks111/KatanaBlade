using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static Player Instance { get; private set; }

    public readonly Inventory Inventory = new();

    private Animator _animator;
    private PlayerController _controller;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;

        Inventory.AddItem(new EssenceOfRegenerate());
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

        if (!_controller.IsConscious)
            _animator.SetTrigger("Hit");
        
        if (HP <= 0)
        {
            StopAllCoroutines();
            _controller.IsConscious = false;
            _animator.SetBool("Dead", true);
        }
    }
    IEnumerator Stun(float stunSec)
    {
        _controller.IsConscious = false;
        yield return new WaitForSeconds(stunSec);
        _controller.IsConscious = true;
    }

}
