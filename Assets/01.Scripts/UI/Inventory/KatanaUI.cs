using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KatanaUI : MonoBehaviour
{
    public float time;
    public Image image;
    public Color essenceColor;
    private Coroutine nowCor;

    void Start()
    {
        FadeIn(time);
    }

    void Update()
    {
        
    }

    private void FadeIn(float time)
    {
        if (nowCor == null)
            nowCor = StartCoroutine(FadeInCor(time));
    }

    private IEnumerator FadeInCor(float time)
    {
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
