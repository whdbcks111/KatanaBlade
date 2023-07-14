using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KatanaUI : MonoBehaviour
{
    public float LightTime;

    public Image Image;
    public Image Light;

    private float _beforeRatio = 0f;
    private Coroutine _cor;

    void Start()
    {
        //FadeIn(time);
    }

    void Update()
    {
        var essence = Player.Instance.Inventory.MountedEssence;
        Color essenceColor = essence is null ? Color.white : ExtraMath.GetMainColor(essence.Icon);


        var ratio = Mathf.Clamp01(Player.Instance.EssenceCooldownRatio);
        Image.color = Color.Lerp(Color.white, essenceColor, ratio);
        
        if(_beforeRatio < 1f && ratio >= 1f)
        {
            //빛나게 함
            if (_cor is not null)
            {
                StopCoroutine(_cor);
            }
            _cor = StartCoroutine(LightRoutine());
        }

        //이전값에 현재값 대입
        _beforeRatio = ratio;
    }

    IEnumerator LightRoutine()
    {
        var essence = Player.Instance.Inventory.MountedEssence;
        Color col = essence is null ? Color.white : ExtraMath.GetMainColor(essence.Icon) * 1.3f;

        for (float i = 0f; i <= 1f; i += Time.deltaTime / LightTime)
        {
            col.a = 1 - i;
            Light.color = col;
            yield return null;
        }

        Light.color = new Color(1, 1, 1, 0);
        _cor = null;
    }
}
