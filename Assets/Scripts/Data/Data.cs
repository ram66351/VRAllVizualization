using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Data
{
    public static Transform Earth;
    public static GameObject Label;
    public DateTime time;
    public float latitide;
    public float longitude;
    public Vector3 posOnSphere;
    
    public GameObject localParent;
    public GameObject Instance3D;
    protected GameObject prefab;
    protected int index;
    protected GameObject label;


    public virtual void CreateMyInstance(int index, Action callback)
    {
        this.index = index;
        Instance3D  = GameObject.Instantiate(prefab, posOnSphere, Quaternion.identity);
        Vector3 dir = new Vector3(0, 1, 0);
        Vector3 crossDir = Vector3.Cross(dir, Instance3D.transform.position);
        float angle = Vector3.Angle(dir, Instance3D.transform.position);
        Instance3D.transform.Rotate(crossDir, angle, Space.Self);
    }

}
