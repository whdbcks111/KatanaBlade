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


        Inventory.AddItem(new EssenceOfFlame());
        Inventory.AddItem(new EssenceOfDarkness());
        Inventory.AddItem(new EssenceOfCloud());
        Inventory.AddItem(new EssenceOfVoid());
        Inventory.AddItem(new BootsOfTraveler());

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
            if(Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() && _essenceRemainCooldown <= 0)
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
