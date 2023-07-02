using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ExtraMath : MonoBehaviour
{
    public float DirectionToAngle(Vector3 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    public Vector2 AngleToDirection(float angle)
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
