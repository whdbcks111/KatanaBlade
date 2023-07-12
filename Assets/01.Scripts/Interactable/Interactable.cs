using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float _interactDistance = 2;

    private void Awake()
    {
        var hint = Instantiate(Resources.Load<Canvas>("UI/InteractHint"));
        hint.transform.SetParent(transform);
        hint.transform.localPosition = Vector3.zero;
        hint.GetComponentInChildren<InteractHint>().InteractDistance = _interactDistance;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && 
            (Player.Instance.transform.position - transform.position).sqrMagnitude < _interactDistance * _interactDistance)
        {
            OnInteract(Player.Instance);
        }
    }

    public abstract void OnInteract(Player player);
}
