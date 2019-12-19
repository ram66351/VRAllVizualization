using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SatelliteObject
{
    public static float factor = 10000;
    public string name;
    public string otherName;
    public string launchTime;
    public string epoch;
    public int perigee;
    public int apogee;
    public float inclination;
    public float period;
    public int mass;
    public int life;
    public string rocket;
    public string block;
    public string prn;
    public string plane;
    public int slot;
    private GameObject prefab;
    private GameObject earth;
    public GameObject gameObject_SatelliteInstance;

    public DateTime LaunchAtOrbitTime;
    public Double TimeStamp;
   

    public SatelliteObject(GameObject prefab, GameObject earth, string name, int perigee, int apogee, string epoch, float inclination, float period, string PRN, string plane, int slot)
    {
        this.name = name;
        this.perigee = perigee;
        this.apogee = apogee;
        this.epoch = epoch;
        this.inclination = inclination;
        this.prn = PRN;
        this.period = period;
        this.prefab = prefab;
        this.earth = earth;
        this.plane = plane;
        this.slot = slot;
      

        if (System.DateTime.TryParse(epoch, out LaunchAtOrbitTime))
        {
            var baseDate = new DateTime(1970, 01, 01);
            var toDate = LaunchAtOrbitTime;
            TimeStamp = toDate.Subtract(baseDate).TotalSeconds;
            //Debug.Log(name + ", " + epoch + ", " + TimeStamp);
        }
        else
        {
            Debug.Log("Unable to convert : " + epoch + ", Satellite name : " + name);
        }
    }

    public void CreateSatellite(int index)
    {
        //gameObject_SatelliteInstance = GameObject.Instantiate(prefab, earth.transform.position, Quaternion.identity) as GameObject;
        //SatellitePhysics satellitePhysics = gameObject_SatelliteInstance.GetComponent<SatellitePhysics>();
        //satellitePhysics.Init(name, (float)perigee/factor, (float)apogee/factor, inclination, period/100.0f, earth.transform,  index, this);

        float radius = (float)perigee / factor;
        float angle = slot * Mathf.PI * 2f / SatelliteHandler.NumberOfSlots;
        Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, InputManager.Instance.transform_Globe.position.y, Mathf.Sin(angle) * radius);

        gameObject_SatelliteInstance = GameObject.Instantiate(prefab, earth.transform.position, Quaternion.identity) as GameObject;//GameObject.Instantiate(prefab);
        gameObject_SatelliteInstance.name = name;
        float offset = 0;

        Debug.Log("plane : " +plane + ", Slot : " + slot + ", Satellite Name : " + name);
        switch (plane)
        {
            case "A":
                gameObject_SatelliteInstance.transform.parent = SatelliteHandler.Instance.PlaneA;
                offset = 0;
                break;

            case "B":
                gameObject_SatelliteInstance.transform.parent = SatelliteHandler.Instance.PlaneB;
                offset = 15;
                break;

            case "C":
                gameObject_SatelliteInstance.transform.parent = SatelliteHandler.Instance.PlaneC;
                offset = 30;
                break;

            case "D":
                gameObject_SatelliteInstance.transform.parent = SatelliteHandler.Instance.PlaneD;
                offset = 45;
                break;

            case "E":
                gameObject_SatelliteInstance.transform.parent = SatelliteHandler.Instance.PlaneE;
                offset = 60;
                break;

            case "F":
                gameObject_SatelliteInstance.transform.parent = SatelliteHandler.Instance.PlaneF;
                offset = 75;
                break;

            default:
                Debug.Log("Not in any plane : " + name + ", myplane : " + plane);
                break;
        }

        //gameObject_SatelliteInstance.transform.localPosition = newPos;
        //Here the value 60 refers to the angle difference between each slotpoints => where each plane has 6 equally spaced slots => 360/60 = 60
        gameObject_SatelliteInstance.transform.localEulerAngles = new Vector3(0, ((slot - 1) * 60) + offset, 0);
        SatellitePhysics satellitePhysics = gameObject_SatelliteInstance.GetComponent<SatellitePhysics>();
        satellitePhysics.Init(name, (float)perigee/factor, (float)apogee/factor, inclination, period/100.0f, earth.transform,  index, this);
        //AlignSatellites(gameObject_SatelliteInstance.transform.parent, offset);
    }

    void AlignSatellites(Transform plane, float offset)
    {
        int count = plane.childCount;
        for(int i=0; i<count; i++)
        {
            plane.GetChild(0).localEulerAngles = new Vector3(0, offset + 360/(i+1), 0);
        }
    }
}
