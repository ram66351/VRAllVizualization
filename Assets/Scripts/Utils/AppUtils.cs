using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppUtils 
{
    
    public static Vector3 LatLonToPositionOnSphere(float lt, float ln, float _radius)
    {
        float ltR = lt * Mathf.Deg2Rad;
        float lnR = ln * Mathf.Deg2Rad;
        float xPos = (_radius) * Mathf.Cos(ltR) * Mathf.Cos(lnR);
        float zPos = (_radius) * Mathf.Cos(ltR) * Mathf.Sin(lnR);
        float yPos = (_radius) * Mathf.Sin(ltR);

        return new Vector3(xPos, yPos, zPos);
    }

    public static float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }


}
