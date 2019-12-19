using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteTrigger : MonoBehaviour
{
    public Material LineMat; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider coll)
    {
        if(!coll.GetComponent<ShowSatelliteInfo>().TurnOff)
        {
            LineRenderer lr = null;
            if (!coll.GetComponent<LineRenderer>())
            {
                lr = coll.gameObject.AddComponent<LineRenderer>();
            }
            else
            {
                lr = coll.gameObject.GetComponent<LineRenderer>();
            }

            if (lr != null)
            {
                lr.useWorldSpace = false;
                lr.material = LineMat;
                lr.SetPosition(0, (transform.position - coll.gameObject.transform.localPosition));//normalized);
                lr.SetPosition(1, new Vector3(0, 0, 0));
                lr.startWidth = 0.02f;
                lr.endWidth = 0.02f;
            }
        }
    }

    void OnTriggerStay(Collider coll)
    {
        LineRenderer lr = coll.gameObject.GetComponent<LineRenderer>();
        if(lr != null)
        {
            lr.SetPosition(0, (transform.position - coll.gameObject.transform.localPosition));//normalized ;
            lr.SetPosition(1, new Vector3(0,0,0));
            //Vector3 dir = (coll.gameObject.transform.localPosition - transform.localPosition).normalized;
            //dir = coll.gameObject.transform.localPosition - dir;
            //lr.SetPosition(1, -dir);
        }
       
    }

    void OnTriggerExit(Collider coll)
    {
        if(coll.gameObject.GetComponent<LineRenderer>())
        {
            Destroy(coll.gameObject.GetComponent<LineRenderer>());
        }
    }
}
