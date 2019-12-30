using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataLabel : MonoBehaviour
{
    private string info;
    public Transform from;
    private LineRenderer lr;
    public Text Info;
    Storm storm;
    StormColorCodeCategory stormCat;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = 0.006f;
        
    }

    public void SetInfo(string info, Transform from)
    {
        Info.text = info;
        this.from = from;
    }

    // Update is called once per frame
    void Update()
    {
        if(from)
        {
            //lr.SetPosition(0, transform.position);
            //lr.SetPosition(1, from.position);
            transform.position = from.position * 1.2f;
        }       
    }
}
