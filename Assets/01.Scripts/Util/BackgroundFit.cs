using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundFit : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Fit();
    }

    private void Update()
    {
        Fit();
    }

    private void Fit()
    {
        var leftBottom = Camera.main.ViewportToWorldPoint(Vector2.zero);
        var rightTop = Camera.main.ViewportToWorldPoint(Vector2.one);

        var boundsSize = _spriteRenderer.bounds.size;
        var spriteRatio = boundsSize.x / boundsSize.y;
        var screenRatio = (rightTop.x - leftBottom.x) / (rightTop.y - leftBottom.y);

        _spriteRenderer.gameObject.transform.position = ((rightTop + leftBottom) / 2f) * Vector2.one;

        if (screenRatio > spriteRatio)
        {
            _spriteRenderer.gameObject.transform.localScale *= ((rightTop.x - leftBottom.x) / boundsSize.x) * 1.02f;
        }
        else
        {
            _spriteRenderer.gameObject.transform.localScale *= ((rightTop.y - leftBottom.y) / boundsSize.y) * 1.02f;
        }
    }
}
