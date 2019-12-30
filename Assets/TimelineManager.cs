using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LineRenderer))]
public class TimelineManager : MonoBehaviour
{
   
    public static TimelineManager Instance;
    public GameObject timelineSlot;
    public float radius = 1f;
    public int numberOfSlot = 24;
    public GameObject Sector;
    private LineRenderer lr;
    public float SectorAngle;
    public string[] monthName = new string[] { "Jan", "Feb", "Mar",
                                              "Apr", "May", "Jun",
                                               "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
    public int segment = 5;
    public int StartTimeStamp;
    public int EndTimeStamp;
    public static DateTime baseDate = new DateTime(2019, 01, 01);
    public static DateTime endDate = new DateTime(2019, 12, 31);

    public float CurrentAngle;
    public float currentRangeMin;
    public float CurrentRangeMax;

    public int MinTimeStamp;
    public int MaxTimeStamp;

    public delegate void CheckIfMyEvent();
    public static CheckIfMyEvent CheckIfMyTimeStampEvent;

    private bool runOnce = true;
    public int totalPointsVisible = 0;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = false;

        Vector3[] points = new Vector3[numberOfSlot + 1];

        for (int i = 0; i < numberOfSlot; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfSlot;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, transform.position.y, Mathf.Sin(angle) * radius);
            GameObject go = Instantiate(timelineSlot, newPos, Quaternion.identity);
            go.transform.parent = transform;
            int index = i / 2;
            string label = monthName[index];
            if (i%2 != 0)
            {
                label += " 15";
            }
            go.GetComponent<TimeslotManager>().Init(i, label);
            points[i] = go.transform.localPosition;
        }
        points[numberOfSlot] = points[0];
        lr.positionCount = numberOfSlot + 1;
        lr.SetPositions(points);

        DrawMySector(segment);
         //transform.eulerAngles = new Vector3(0, 90, 0);

         StartTimeStamp = (int) baseDate.Subtract(baseDate).TotalSeconds;
        EndTimeStamp = (int) endDate.Subtract(baseDate).TotalSeconds;
    }

    public void DrawOnSliderChanged(float val)
    {
        segment = (int) val;
        DrawMySector(segment);
        BroadcastEvent();
    }

    private void DrawMySector(int _segment)
    {
        Destroy(Sector.GetComponent<MeshFilter>().mesh);
        Mesh m = AppUtils.GenerateCircleMesh(_segment);
        Sector.GetComponent<MeshFilter>().mesh = m;
        SectorAngle = AppUtils.angle * Mathf.Rad2Deg;
        Sector.transform.eulerAngles = new Vector3(0, 90 - (_segment * 15) / 2, 0);     
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.rotation = Quaternion.Euler(0, InputManager.Instance.transform_Globe.eulerAngles.y, 0);
        if (AppModeManager.Instance.appMode == AppModeManager.AppMode.time)
        {
            float speed = 100.0f;
            if (InputManager.Instance.GetKey(InputManager.ControllerInput.btn_touchpad))
            {
                Quaternion q = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
                //q.Set(q.x * -1, q.y * -1, q.z, q.w);
                //transform.rotation =  Quaternion.Slerp(transform.rotation, q, 1.0f * );
                //transform.eulerAngles = q.eulerAngles;               
                transform.Rotate(0, -q.y * speed * Time.deltaTime, 0, Space.World);

                if (runOnce)
                {
                    BroadcastEvent();
                    runOnce = false;
                }
            }
            else
            {
                if (!runOnce)
                {  
                    runOnce = true;
                    BroadcastEvent();
                }
            }

            if(Input.mouseScrollDelta.y != 0)
            {
                transform.Rotate(0, Input.mouseScrollDelta.y * speed * Time.deltaTime, 0, Space.World);

                BroadcastEvent();
            }

            
        }
    }

    public void BroadcastEvent()
    {
        Debug.Log("BroadcastEvent");
        if ((transform.eulerAngles.y - 90) > 0)
            CurrentAngle = transform.eulerAngles.y - 90;
        else
            CurrentAngle = 270 + transform.eulerAngles.y;

        float wideAngle = segment * 15;

        if ((CurrentAngle - wideAngle / 2) > 0)
            currentRangeMin = CurrentAngle - wideAngle / 2;
        else
            currentRangeMin = 360 - (CurrentAngle - wideAngle / 2);

        if ((CurrentAngle + wideAngle / 2) < 360)
            CurrentRangeMax = CurrentAngle + wideAngle / 2;
        else
            CurrentRangeMax = (CurrentAngle + wideAngle / 2) - 360;

        MinTimeStamp = (int) AppUtils.Remap(currentRangeMin, 0, 360, StartTimeStamp, EndTimeStamp);
        MaxTimeStamp = (int) AppUtils.Remap(CurrentRangeMax, 0, 360, StartTimeStamp, EndTimeStamp);

        CheckIfMyTimeStampEvent();
    }
}
