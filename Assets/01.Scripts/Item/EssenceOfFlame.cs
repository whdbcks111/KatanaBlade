using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EssenceOfFlame : Item
{
    public GameObject bullet;

    public float ActiveRadius = 15;
    private static readonly float ActiveDamage = 5f;

    public float PassiveTick;
    public float PassiveRadius;
    private static readonly float PassiveDamage = 5f;
    private float _lastUsed = -1;
    private static readonly float Cooldown = 5f;
    private float _dT;

    public EssenceOfFlame()
        : base(ItemType.Essence, "화염의 정수",
            string.Format(
                "사용 시 : 주변 적에게 투사체를 날려 <color=red>{0}</color>만큼 피해를 입히고 <color=red>{1}</color>만큼 지속피해를 입힙니다.\n" +
                " <color=gray>(재사용 대시기간 : {2:0.0}초)</color>\n" +
                "기본 지속 효과 : 주변 적에게 <color=red>{3}</color> 만큼 지속피해를 입힙니다.", ActiveDamage, PassiveDamage, PassiveDamage, Cooldown),
            Resources.Load<Sprite>("Item/Icon/EssenceOfRegeneration"))
    {
    }

    [ContextMenu("액티브 사용")]
    public override void OnActiveUse()
    {
        if (_lastUsed > 0 && (Time.realtimeSinceStartup - _lastUsed) < Cooldown) return;
        _lastUsed = Time.realtimeSinceStartup;

        //적 레이어 추가해야함
        Collider2D[] enemies = Physics2D.OverlapCircleAll(Player.Instance.transform.position, ActiveRadius);
        
        if(enemies.Length > 0)
        {
            int minDist = 0;
            for (int i = 0; i < enemies.Length; i++) 
            {
                if (enemies[i].GetComponent<Entity>() is Monster && Vector2.Distance(Player.Instance.transform.position, enemies[i].transform.position) < Vector2.Distance(Player.Instance.transform.position, enemies[minDist].transform.position))
                {
                    minDist = i;
                }
            }
            bullet.transform.position = Player.Instance.transform.position;
            Tilemap map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
            StartCoroutine(ChaseTarget(map, bullet.transform, enemies[minDist].transform.position));
            //적에게 투사체 발사 코드, 맞은 적에게 대미지 입히는 코드
        }
    }

    public override void PassiveUpdate()
    {
        _dT += Time.deltaTime;
        if(_dT > PassiveTick)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(Player.Instance.transform.position, PassiveRadius, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (var enemy in enemies)
            {
                if (enemy.GetComponent<Entity>() is Monster)
                {
                    enemy.GetComponent<Entity>().Damage(PassiveDamage);
                }
            }
            _dT = 0;
        }
    }

    private IEnumerator ChaseTarget(Tilemap tilemap, Transform obj, Vector2 target)
    {
        while (Vector2.Distance((Vector2)obj.transform.position, target) > .1f)
        {
            Pathfinder.Follow(tilemap, obj, target, 6);
            yield return null;
        }
    }

    public override void OnMount()
    {
    }

    public override void OnUnmount()
    {
    }
}
