using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Entity
{
    public static Player Instance { get; private set; }

    public readonly Inventory Inventory = new();

    private PlayerController _controller;
    private Animator _animator;
    private float _essenceCooldown = 1f, _essenceRemainCooldown = 0f;

    public float EssenceCooldownRatio => 1 - _essenceRemainCooldown / _essenceCooldown;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;

        Inventory.AddItem(new EssenceOfSwift());
        Inventory.AddItem(new EssenceOfLight());
        Inventory.AddItem(new EssenceOfStorm());
        Inventory.AddItem(new EssenceOfVoid());
        Inventory.AddItem(new EssenceOfFlame());
        Inventory.AddItem(new EssenceOfVoid());
        Inventory.AddItem(new FairyCloak());
        Inventory.AddItem(new MagicScroll());

        _controller = GetComponent<PlayerController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    protected override void Update()
    {
        base.Update();

        if (Inventory.MountedAccessory is not null) Inventory.MountedAccessory.PassiveUpdate();
        if (Inventory.MountedEssence is not null)
        {
            Inventory.MountedEssence.PassiveUpdate();
            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() && _essenceRemainCooldown <= 0)
            {
                Inventory.MountedEssence.OnActiveUse();
            }
        }

        if (_essenceRemainCooldown > 0) _essenceRemainCooldown -= Time.deltaTime;
    }

    public void SetEssenceCooldown(float time)
    {
        _essenceCooldown = _essenceRemainCooldown = time * Instance.Stat.Get(StatType.EssenceCooldown);
    }

    public override void Damage(float damageAmount)
    {
        if (HP <= 0)
        {
            StopAllCoroutines();
            _controller.IsConscious = false;
            MovingVelocity = 0;
            _animator.SetBool("Dead", true);
            GameManager.instance.OnPlayerDead();
        }
        else if (!_controller.IsParrying)
        {
            base.Damage(damageAmount);
            _animator.SetTrigger("Hit");
        }
    }
}
