using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColor : MonoBehaviour
{
    public Sprite sprite;
    public Color result;

    private void Start()
    {
        float r = 0, g = 0, b = 0;
        int count = 0;
        var xAdd = sprite.texture.width / 10;
        var yAdd = sprite.texture.height / 10;

        for (int x = 0; x < sprite.texture.width; x += xAdd)
        {
            for (int y = 0; y < sprite.texture.height; y += yAdd)
            {
                Color color = sprite.texture.GetPixel(x, y);
                r += color.r;
                b += color.b;
                g += color.g;
                ++count;
            }
        }

        var maxValue = Mathf.Max(r, g, b);

        print(r + "," + g + "," + b);
        r /= maxValue;
        g /= maxValue;
        b /= maxValue;
        var averColor = new Color(r, g, b);

        result = averColor;
    }
}
