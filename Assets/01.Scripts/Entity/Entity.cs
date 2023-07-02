using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    
    public virtual void Attack(Entity other)
    {
        var damage = 1; // 예시 값, 실제로는 계산
        other.Damage(damage);
    }

    public virtual void Damage(float damage)
    {
        // HP 닳는 코드 구현
    }
}
