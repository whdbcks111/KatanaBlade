using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeStepper : MonoBehaviour
{
    [SerializeField] private List<GameObject> _trapList;
    //public GameObject TrapKnife;
    public float MinLimit;
    public float MaxLimit;
    public void Shuffle()
    {
        foreach (GameObject obj in _trapList)
            obj.transform.position = new Vector2(Random.Range(MinLimit, MaxLimit), obj.transform.position.y);
    }

}
