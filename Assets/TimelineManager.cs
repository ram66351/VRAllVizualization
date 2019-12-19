using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TimelineManager : MonoBehaviour
{
    public GameObject timelineSlot;
    public float radius = 1f;
    private int numberOfSlot = 24;

    private LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = false;

        Vector3[] points = new Vector3[numberOfSlot + 1];

        for (int i = 0; i < numberOfSlot; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfSlot;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, transform.position.y, Mathf.Sin(angle) * radius);
            GameObject go = Instantiate(timelineSlot, newPos, Quaternion.identity);
            go.transform.parent = transform;
            go.GetComponent<TimeslotManager>().Init(24 - i);
            points[i] = go.transform.localPosition;
        }
        points[numberOfSlot] = points[0];
        lr.positionCount = numberOfSlot + 1;
        lr.SetPositions(points);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, InputManager.Instance.transform_Globe.eulerAngles.y, 0);
    }
}
