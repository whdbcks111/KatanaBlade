using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeStepper : MonoBehaviour
{
    [SerializeField]private List<GameObject> _trapList;
    //public GameObject TrapKnife;
    [SerializeField] private float _minLimit;
    [SerializeField] private float _maxLimit;
    public void Shuffle()
    {
        foreach (GameObject obj in _trapList)
            obj.transform.position = new Vector2(Random.Range(_minLimit, _maxLimit), obj.transform.position.y);
    }

}
