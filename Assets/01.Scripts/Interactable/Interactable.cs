using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float _interactDistance = 2;

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
