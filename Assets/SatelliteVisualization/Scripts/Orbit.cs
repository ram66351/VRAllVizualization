using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Orbit
{
    public float xAxis;
    public float yAxis;
    public Vector3 center;

    public Orbit(float xAxis, float yAxis, Vector3 center)
    {
        this.xAxis = xAxis;
        this.yAxis = yAxis;
        this.center = center;
    }

    public Vector2 Evaluate(float t)
    {
        float angle = t * 360f * Mathf.Deg2Rad;
        float x = center.x + Mathf.Sin(angle) * xAxis;
        float y = center.z + Mathf.Cos(angle) * yAxis;
        return new Vector2(x, y);
    }

    public void MoveAroundOrbit(Transform t, float alpha)
    {
        t.localPosition = new Vector3(center.x + xAxis * Mathf.Cos(alpha), 0, center.z + yAxis * Mathf.Sin(alpha));
    }
}
