using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Storm : Data
{
    public string serialNo;
    public string season;
    public string number;
    public string basin;
    public string subBasin;
    public string name;
    public string nature;
    public string wind;
    public string pressure;
    public string center;
    public string windPercentile;
    public string pressurePercentile;
    public string trackType;
    public double convertedTimestamp;


    public Storm(float lat, float lon, GameObject _prefab, DateTime t, GameObject _label)
    {
        latitide = lat;
        longitude = lon;
        time = t;
        prefab = _prefab;
        label = _label;
        posOnSphere = AppUtils.LatLonToPositionOnSphere(latitide, longitude, Earth.localScale.x / 4.0f);
        //gameObject = GameObject.Instantiate(prefab, posOnSphere * 2, Quaternion.identity) as GameObject;
        //parentGameobject = parent;
        //gameObject.transform.SetParent(parentGameobject);
    }

    public override void CreateMyInstance(int index, Action callback)
    {
        base.CreateMyInstance(index, callback);
        Instance3D.name = name + "_" + index;
        //Instance3D.GetComponent<StromDataPoint>().Init(this, label);
        Instance3D.GetComponent<StormInstance>().Init(this);
        callback();
    }
}
