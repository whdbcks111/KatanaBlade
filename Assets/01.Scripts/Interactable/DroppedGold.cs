using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DroppedGold : MonoBehaviour
{
    public int GoldQuantities;
    public int MaxGoldQuantities;
    public int MinGoldQuantities;

    private void Start()
    {
        GoldQuantities = Random.Range(MinGoldQuantities, MaxGoldQuantities);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Entity>() is Player)
        {
            Destroy(this.gameObject);
        }
    }

}

