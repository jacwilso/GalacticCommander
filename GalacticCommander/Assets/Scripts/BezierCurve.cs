using CatlikeCoding;
using UnityEngine;

public class BezierCurve
{
    public Vector3[] points;

    public BezierCurve(Vector3[] points)
    {
        this.points = points;
    }

    public Vector3 GetPoint(float t)
    {
        return Bezier.GetPoint(points[0], points[1], points[2], points[3], t);
    }

    public Vector3 GetVelocity(float t)
    {
        return Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t);
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }
}
