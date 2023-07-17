using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfLight : Item
{
    private static readonly float Cooldown = 3f;
    private static readonly float MaintainTime = 3f;
    private static readonly float ActiveLength = 25f;
    private float _dT = 0f;
    private LineRenderer _line;
    private Material _material;

    public EssenceOfLight()
        : base(ItemType.Essence, "빛의 정수",
            string.Format(
                "사용 시 : HP를 <color=green>10</color> 회복합니다. <color=#aaa>(재사용 대시기간 : {0:0.0}초)</color>\n" +
                "기본 지속 효과 : 1초당 HP를 <color=green>2</color> 회복합니다.", Cooldown),
            Resources.Load<Sprite>("Item/Icon/Essence/Essence_4"))

    {

    }

    public override void OnActiveUse()
    {
        Player.Instance.SetEssenceCooldown(Cooldown);

        Player.Instance.StartCoroutine(SkillCor(MaintainTime));
    }

    public override void PassiveUpdate()
    {
    }

    public override void OnMount()
    {
        _material = Resources.Load<Material>("Item/LightLine");
    }

    public override void OnUnmount()
    {
    }

    private IEnumerator SkillCor(float maintainTime)
    {
        float dT = 0;
        if(_line == null)
        {
            _line = Player.Instance.gameObject.AddComponent<LineRenderer>();

        }
        _line.material = _material;
        _line.textureMode = LineTextureMode.Tile;
        _line.startColor = _line.endColor = Color.white;
        _line.startWidth = _line.endWidth = 1f;
        _line.positionCount = 2;
        while (dT < maintainTime)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var hit = Physics2D.Raycast(Player.Instance.transform.position, (mousePos - (Vector2)Player.Instance.transform.position).normalized,
                ActiveLength, LayerMask.GetMask("Platform", "Enemy"));

            float angle = ExtraMath.DirectionToAngle(((Vector2)Player.Instance.transform.position - mousePos).normalized);

            _line.SetPosition(0, Player.Instance.transform.position);
            if (hit)
            {
                _line.SetPosition(1, hit.point);
            }
            else
            {
                _line.SetPosition(1, new Vector2((-Mathf.Cos(angle * Mathf.Deg2Rad) * ActiveLength) + Player.Instance.transform.position.x,
                (-Mathf.Sin(angle * Mathf.Deg2Rad) * ActiveLength) + Player.Instance.transform.position.y));
            }
            yield return null;
            dT += Time.deltaTime;
        }
        _line.SetPosition(0, Vector3.zero);
        _line.SetPosition(1, Vector3.zero);
    }


}
