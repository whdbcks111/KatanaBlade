using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public float DeadTime;
    [HideInInspector]
    public float Angle;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DeadTime);
        transform.eulerAngles = new Vector3 (0, 0, Angle);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
    }
}
