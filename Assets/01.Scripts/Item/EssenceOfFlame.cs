using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EssenceOfFlame : Item
{

    public float ActiveRadius = 15;
    private static readonly float ActiveDamage = 5f;
    private static readonly float ActiveTotalTime = 2f;

    public float PassiveTick;
    public float PassiveRadius;
    private static readonly float PassiveDamage = 5f;
    private static readonly float Cooldown = 5f;
    private float _dT;

    public EssenceOfFlame()
        : base(ItemType.Essence, "화염의 정수",
            string.Format(
                "사용 시 : 주변 적에게 불꽃을 날려 <color=red>{0}</color>만큼 피해를 입히고 <color=red>{1}</color>만큼 지속피해를 입힙니다.\n" +
                " <color=gray>(재사용 대시기간 : {2:0.0}초)</color>\n" +
                "기본 지속 효과 : 주변 적에게 초당 <color=red>{3}</color>만큼 피해를 입힙니다.", ActiveDamage, PassiveDamage, Cooldown, PassiveDamage),
            Resources.Load<Sprite>("Item/Icon/Essence/Essence_2"))
    {
    }

    [ContextMenu("액티브 사용")]
    public override void OnActiveUse()
    {
        Player.Instance.SetEssenceCooldown(Cooldown);

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
            FlyingProjectile projectile = Instantiate(Resources.Load("Item/FlameProjectile"), Player.Instance.transform.position, Quaternion.identity).GetComponent<FlyingProjectile>();

            Tilemap map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
            Player.Instance.StartCoroutine(ChaseTarget(map, projectile, enemies[minDist].transform));
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

    private IEnumerator ChaseTarget(Tilemap tilemap, FlyingProjectile obj, Transform target)
    {
        obj.tilemap = tilemap;
        obj.owner = Player.Instance;
        obj.target = target;
        yield return new WaitUntil(() => Vector2.Distance((Vector2)obj.transform.position, target.transform.position) < .1f);

        target.GetComponent<Entity>().Damage(ActiveDamage);
        float dT = 0;
        while(dT < ActiveTotalTime)
        {
            dT += Time.deltaTime;
            if (target.GetComponent<Entity>().HP > 0)
            {
                target.GetComponent<Entity>().Damage(PassiveDamage / ActiveTotalTime * Time.deltaTime);
            }
            else
            {
                break;
            }
        }
        Destroy(obj.gameObject);
    }

    public override void OnMount()
    {
    }

    public override void OnUnmount()
    {
    }
}
