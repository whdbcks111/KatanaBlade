using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KatanaUI : MonoBehaviour
{
    public float time;
    public Image image;
    private Coroutine nowCor;

    private float _timer = 0f;
    private Color _startColor, _endColor;

    void Start()
    {
        //FadeIn(time);
    }

    void Update()
    {
        var essence = Player.Instance.Inventory.MountedEssence;
        Color essenceColor = essence is null ? Color.white : ExtraMath.GetMainColor(essence.Icon);
        if (_endColor != essenceColor)
        {
            _timer = 0f;
            _startColor = image.color;
            _endColor = essenceColor;
        }

        if(_timer < time)
        {
            image.color = Color.Lerp(_startColor, _endColor, Mathf.Clamp01(_timer / time));
            _timer += Time.deltaTime;
        }
        
    }

    private void FadeIn(float time)
    {
        if (nowCor == null)
            nowCor = StartCoroutine(FadeInCor(time));
    }

    private IEnumerator FadeInCor(float time)
    {
        Color essenceColor = ExtraMath.GetMainColor(Player.Instance.Inventory.MountedEssence.Icon);
        Color origin = new Color(essenceColor.r, essenceColor.g, essenceColor.b, 0);
        float dT = 0;
        while (true)
        {
            if (dT > time)
                break;

            dT += Time.deltaTime;
            yield return null;

            float a = dT / time;
            Color temp = new Color(origin.r, origin.g, origin.b, a);
            image.color = temp;
        }

        image.color = essenceColor;
        nowCor = null;
    }
}
