using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Earthquake : Data
{
    public float depth;
    public float mag;
    public string magType;
    public string place;

    public Earthquake(float _lat, float _lon, DateTime _time, float _mag, string _magType, GameObject _prefab, GameObject _label)
    {
        latitide = _lat;
        longitude = _lon;
        mag = _mag;
        time = _time;
        magType = _magType;
        prefab = _prefab;
        label = _label;
        posOnSphere = AppUtils.LatLonToPositionOnSphere(latitide, longitude, Earth.localScale.x / 2.0f);
    }

    public override void CreateMyInstance(int index, Action callback)
    {
        base.CreateMyInstance(index, callback);
        Instance3D.name = "Earthquake_" + index;
        //Instance3D.GetComponent<EarthquakeDataPoint>().Init(this, label);
        callback();
    }
}
