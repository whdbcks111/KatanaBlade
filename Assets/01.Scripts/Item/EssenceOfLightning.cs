using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfLightning : Item
{
    
    private List<Entity> _entities = new List<Entity>();

    private LineRenderer _line;
    private Material _material;
    private static readonly float Cooldown = 5f;
    private static readonly float EssenceRadius = 10f;
    private static readonly int ActiveCount = 5;
    private static readonly int ActiveForce = 10;

    public EssenceOfLightning()
        : base(ItemType.Essence, "번개의 정수",
            string.Format(
                "사용 시 : 주변 적 {0}명에게 <color=yellow>번개</color>를 내려 {1}의 대미지를 입히고 기절시킵니다. <color=gray>(재사용 대시기간 : {2:0.0}초)</color>\n" +
                "기본 지속 효과 : - ", ActiveCount, ActiveForce, Cooldown),
            Resources.Load<Sprite>("Item/Icon/Essence/Essence_1"))
    {
    }

    [ContextMenu("액티브 사용")]
    public override void OnActiveUse()
    {
        _entities.Clear();
        //기준 몬스터 찾기
        Collider2D[] enemies = Physics2D.OverlapCircleAll(Player.Instance.transform.position, EssenceRadius, 1 << LayerMask.NameToLayer("Enemy"));
        int min = 0;
        if(enemies.Length > 0)
        {
            Player.Instance.SetEssenceCooldown(Cooldown);
            for (int i = 0; i < enemies.Length; i++)
            {
                //가장 가까운 적 찾기
                if (Vector2.Distance(Player.Instance.transform.position, enemies[i].transform.position) < Vector2.Distance(Player.Instance.transform.position, enemies[min].transform.position))
                {
                    min = i;
                }
            }
            if (enemies[min] != null)
            {
                _entities.Add(enemies[min].GetComponent<Entity>());
                Lightning(enemies[min].GetComponent<Entity>(), ActiveCount - 1, ActiveForce);
            }
        }
    }
    public override void PassiveUpdate()
    {
    }

    public void Lightning(Entity entity, int cnt, float damage)
    {
        if(cnt <= 0)            //재귀호출 종료시(주변 적 없다면)
        {
            if(_line == null)
            {
                _line = Player.Instance.gameObject.AddComponent<LineRenderer>();
            }
            _line.material = _material;
            _line.textureMode = LineTextureMode.Tile;
            _line.startColor = _line.endColor = Color.white;
            _line.startWidth = _line.endWidth = 1f;
            _line.positionCount = _entities.Count + 1;
            _line.SetPosition(0, Player.Instance.transform.position);
            Debug.Log(_entities.Count);
            for (int i = 0; i < _entities.Count; i++)
            {
                _line.SetPosition(i + 1, _entities[i].transform.position);
                _entities[i].Damage(damage * Player.Instance.Stat.Get(StatType.EssenceForce));
                _entities[i].AddEffect(new EffectStun(1, 2f * Player.Instance.Stat.Get(StatType.EssenceForce), Player.Instance));
                Player.Instance.StartCoroutine(LightningAnim(.3f));
            }
        }
        else        //주변 적 탐색, 재귀 호출
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(entity.transform.position, EssenceRadius, 1 << LayerMask.NameToLayer("Enemy"));
            if(enemies.Length > 0)
            {
                int min = 0;
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (Vector2.Distance(entity.transform.position, enemies[i].transform.position) > Vector2.Distance(entity.transform.position, enemies[min].transform.position)
                        && enemies[i].GetComponent<Entity>() != entity
                        && !_entities.Contains(enemies[i].GetComponent<Entity>()))
                        min = i;
                }
                //min값이 본인을 갖진 않음

                for (int i = 0; i < enemies.Length; i++)
                {
                    if (Vector2.Distance(entity.transform.position, enemies[i].transform.position) < Vector2.Distance(entity.transform.position, enemies[min].transform.position)
                        && enemies[i].GetComponent<Entity>() != entity
                        && !_entities.Contains(enemies[i].GetComponent<Entity>()))
                    {
                        min = i;
                    }
                }
                if (_entities.Contains(enemies[min].GetComponent<Entity>()))
                {
                    Lightning(entity, 0, damage);
                }
                else
                {
                    _entities.Add(enemies[min].GetComponent<Entity>());
                    Lightning(enemies[min].GetComponent<Entity>(), cnt - 1, damage);
                }
            }
            else
            {
                Lightning(entity, 0, damage);
            }
        }
    }

    private IEnumerator LightningAnim(float time)
    {
        float dT = 0;
        while (dT < time)
        {
            float width = Mathf.Lerp(1.5f, 0f, dT / time);
            _line.startWidth = width;
            _line.endWidth = width;
            dT += Time.deltaTime;
            yield return null;
        }
        _line.startWidth = _line.endWidth = 0;
        for (int i = 0; i < _line.positionCount; i++)
        {
            _line.SetPosition(i, Vector2.zero);
        }
    }

    public override void OnMount()
    {
        _material = Resources.Load<Material>("Item/LightningLine");
    }

    public override void OnUnmount()
    {
    }
}
