using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.U2D;

public static class ExtraMath
{
    public static float DirectionToAngle(Vector3 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    public static Color GetMainColor(Sprite sprite)
    {
        float r = 0, g = 0, b = 0;
        int count = 0;
        var minPos = sprite.textureRect.min;
        var maxPos = sprite.textureRect.max;
        var xAdd = (int)sprite.textureRect.width / 20;
        var yAdd = (int)sprite.textureRect.height / 20;

        for (int x = (int)minPos.x; x <= (int)maxPos.x; x += xAdd)
        {
            for (int y = (int)minPos.y; y < (int)maxPos.y; y += yAdd)
            {
                Color color = sprite.texture.GetPixel(x, y);

                r += color.r;
                b += color.b;
                g += color.g;
                ++count;
            }
        }

        r /= count;
        g /= count;
        b /= count;
        var mainCol = new Color(r, g, b);

        Color.RGBToHSV(mainCol, out float h, out float s, out float v);
        s = Mathf.Clamp01(s + 0.3f);
        return Color.HSVToRGB(h, s, v);
    }

    public static Vector2 AngleToDirection(float angle)
    {
        return new(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    public static float GetNormalAngle(float angle)
    {
        return (angle % 360f + 360f) % 360f;
    }
    public static bool IsAngleBetween(float target, float from, float to)
    {
        target = GetNormalAngle(target);
        var range = GetAngleRange(from, to);
        from = range.From;
        to = range.To;
        if (from <= to) return from <= target && target <= to;
        return from <= target || target <= to;
    }

    public static AngleRange GetAngleRange(float from, float to)
    {
        from = GetNormalAngle(from);
        to = GetNormalAngle(to);
        var rAngle = GetNormalAngle(to - from);
        if (rAngle > 180)
        {
            (from, to) = (to, from);
        }
        return new(from, to);
    }
}

public struct AngleRange
{
    public float From, To;

    public AngleRange(float from, float to)
    {
        From = from;
        To = to;
    }
}
