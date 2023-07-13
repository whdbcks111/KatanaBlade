using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed = 3f;
    [HideInInspector]
    public Entity owner;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Speed * Time.deltaTime * Vector3.right);
       
    }

    public void SetOwner(Entity entity, float angle)
    {
        owner = entity;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Entity entity) && owner != collision.gameObject.GetComponent<Entity>())
        {
            entity.Damage(10f);
            Destroy(gameObject, 0f);
        }
    }
}
