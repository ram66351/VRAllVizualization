using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSatelliteInfo : MonoBehaviour
{
    public TextMesh textMesh;
    public SatellitePhysics satellitePhysics;
    public SatelliteObject obj;
    public bool TurnOff = false;
    public float DistanceFromCamera;
    float maxLimit = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DistanceFromCamera = Vector3.Distance(textMesh.transform.position, Camera.main.transform.position);
        if (DistanceFromCamera > maxLimit)
            DistanceFromCamera = maxLimit;
        Color col = textMesh.color;
        col.a = AppUtils.Remap(DistanceFromCamera, 0, maxLimit, 1, 0);
        textMesh.color = col;
    }

    private void OnMouseEnter()
    {
        ShowInfo();
    }

    private void OnMouseDown()
    {
        TurnOnOFF();
    }

    public void ShowInfo()
    {
        string info = "";
        if (obj == null)
        {
            obj = satellitePhysics.satelliteObject;
        }
        else
        {
            info = "Name : " + obj.name +
                     "\n Epoch : " + obj.epoch +
                      "\n Perigee : " + obj.perigee +
                      "\n Apogee : " + obj.apogee +
                      "\n Inclination : " + obj.inclination +
                       "\n Mass : " + obj.mass +
                       "\n Life : " + obj.life +
                       "\n Plane : " + obj.plane +
                       "\n Slot : " + obj.slot;
        }

        textMesh.text = info;
        StartCoroutine(HideInfo());

    }

    IEnumerator HideInfo()
    {
        yield return new WaitForSeconds(3);
        if (obj == null)
        {
            obj = satellitePhysics.satelliteObject;
        }
        textMesh.text = obj.name;
    }

    public void TurnOnOFF()
    {
        TurnOff = !TurnOff;

        LineRenderer lr = GetComponent<LineRenderer>();

        if (TurnOff)
        {
            //textMesh.text = obj.name + "\n Disabled !";
            textMesh.color = Color.red;
            satellitePhysics.SpotLight.gameObject.SetActive(false);
            if(lr != null)
                lr.enabled = false;
        }
        else
        {
            //textMesh.text = obj.name;
            satellitePhysics.SpotLight.gameObject.SetActive(true);
            if (lr != null)
                lr.enabled = true;
            textMesh.color = Color.white;
        }
    }
}
