using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[ExecuteInEditMode]
public class UIBezierCurve : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public Transform point3;
    public UILineRenderer ui_lr;
    public int vertexCount = 12;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var pointList = new List<Vector2>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(point1.position, point2.position, ratio);
            var tangentLineVertex2 = Vector3.Lerp(point2.position, point3.position, ratio);
            var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(new Vector2(bezierpoint.x, bezierpoint.y));
        }
       // ui_lr.Points = pointList.Count;
       // ui_lr.SetPositions(pointList.ToArray());

        ui_lr.Points = pointList.ToArray();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(point1.position, point2.position);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(point2.position, point3.position);

        Gizmos.color = Color.red;
        for (float ratio = 0.5f / vertexCount; ratio < 1; ratio += 1.0f / vertexCount)
        {
            Gizmos.DrawLine(Vector3.Lerp(point1.position, point2.position, ratio), Vector3.Lerp(point2.position, point3.position, ratio));
        }
    }
}