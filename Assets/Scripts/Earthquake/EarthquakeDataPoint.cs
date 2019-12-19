using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeDataPoint : DataPoints
{
    public Earthquake quake;
    public void Init(Earthquake _quake, GameObject _label)
    {
        quake = _quake;
        //Assign highlight material to earthquake points
        Label = _label;
        //InitLabel(quake, stromColCat);
        //Vector3 pos = transform.localPosition;
        //transform.localPosition = new Vector3(pos);
    }
}
