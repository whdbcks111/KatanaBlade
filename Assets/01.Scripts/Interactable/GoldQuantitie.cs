using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoldQuantitie : MonoBehaviour
{
    public int Value = 5;
    public float Speed = 10;

    private void Start()
    {
        StartCoroutine(GoldMoveRoutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Player player))
        {
            GameManager.instance.Gold += (int)(Value * player.Stat.Get(StatType.GoldObtainMultiplier));
            Destroy(gameObject);
        }
    }

    private IEnumerator GoldMoveRoutine()
    {
        float angle = Random.Range(0, 360);
        Vector3 direction = ExtraMath.AngleToDirection(angle);

        for (float i = 0f; i < 1f; i += Time.deltaTime / 0.5f)
        {
            transform.position += (1 - i) * Speed * Time.deltaTime * direction;
            yield return null;
        }

        Vector3 _vel = Vector3.zero;
        float smoothTime = 1f;
        while (true)
        {
            smoothTime -= Time.deltaTime * 0.2f * Speed;
            transform.position = Vector3.SmoothDamp(transform.position, Player.Instance.transform.position, ref _vel, smoothTime);
            yield return null;
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

