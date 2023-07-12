using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoldQuantitie : MonoBehaviour
{
    public int value = 5;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            GoldCounter.Instance.IncreaseGolds(value);
        } 
    } 
}
    
        
    





















































//{
//    public int GoldQuantities;
//    public int MaxGoldQuantities;
//    public int MinGoldQuantities;

//    private void Start()
//    {
//        GoldQuantities = Random.Range(MinGoldQuantities, MaxGoldQuantities);
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.GetComponent<Entity>() is Player)
//        {
//            Destroy(this.gameObject);
//        }
//    }

//}

