using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceOfEarth : Item
{
    private static readonly float Cooldown = 5f;
    private static readonly float PassiveTick = 5f;
    private static readonly float ActiveMag = 10f;
    private static readonly float ActiveRadius = 15f;
    private float _dT;

    public EssenceOfEarth()
    : base(ItemType.Essence, "������ ����",
        string.Format(
            "��� �� : �ֺ� ������ <color=brown>�о��</color> ������ŵ�ϴ�. <color=gray>(���� ��ñⰣ : {0:0.0}��)</color>\n" +
            "�⺻ ���� ȿ�� : {1}�ʸ��� ������ ������ �ֺ� ���� <color=brown>����</color>��ŵ�ϴ�.", Cooldown, PassiveTick),
        Resources.Load<Sprite>("Item/Icon/Essence/Essence_0"))
    {
    }

    [ContextMenu("��Ƽ�� ���")]
    public override void OnActiveUse()
    {
        Player.Instance.SetEssenceCooldown(Cooldown);

        Player.Instance.StartCoroutine(ActiveCameraShake(2f, 1f, 0.5f));
        Collider2D[] area = Physics2D.OverlapCircleAll(Player.Instance.transform.position, ActiveRadius, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (var enemy in area)
        {
            if(enemy.GetComponent<Entity>() is Monster)
            {
                enemy.GetComponent<Entity>().AddEffect(new EffectStun(1, 2f, Player.Instance));
                //������ �Ÿ��� ���� ����� �޶���
                enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (ActiveMag + (ActiveRadius - Vector2.Distance(enemy.transform.position, Player.Instance.transform.position))), ForceMode2D.Impulse);
                enemy.GetComponent<Entity>().Knockback(ActiveMag * Random.Range(-1, 2) * 2);

                RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, Vector2.down, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Platform"));
                if (hit)
                {
                    Debug.Log(hit.transform.name);
                    EffectManager.EffectOneShot("Ground Paticle", hit.point);
                }
            }
        }
    }

    public override void PassiveUpdate()
    {
        _dT += Time.deltaTime;
        if(_dT > PassiveTick)
        {
            Collider2D[] area = Physics2D.OverlapBoxAll(Player.Instance.transform.position, new Vector2(ActiveRadius * 2, 2f), 0f, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (var enemy in area)
            {
                enemy.GetComponent<Entity>().AddEffect(new EffectStun(1, .5f, Player.Instance));
            }
            _dT = 0;
        }
    }

    private IEnumerator ActiveCameraShake(float duration, float maxShake, float minShake)
    {
        float dT = 0;
        while(dT < duration)
        {
            Camera.main.GetComponent<CameraControll>().Shake(Time.deltaTime, Mathf.Lerp(maxShake, minShake, dT / duration));
            dT += Time.deltaTime;
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
