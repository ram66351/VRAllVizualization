using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataLabel : MonoBehaviour
{
    private string info;
    public Transform from;
    private LineRenderer lr;
    public SpriteRenderer spriteRenderer;
    public Text Speed;
    public Text Pressure;
    public Text Description;
    Storm storm;
    StormColorCodeCategory stormCat;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = 0.006f;
        
    }

    public void SetInfo(Storm _storm, StormColorCodeCategory _stormCat)
    {
        storm = _storm;
        stormCat = _stormCat;
        Speed.text = "Speed : "+ storm.wind + " KMPH";
        Pressure.text = "Pressure : " + storm.pressure;
        Description.text = _stormCat.Description;
        spriteRenderer.sprite = _stormCat.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(from)
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, from.position);
        }
        
    }
}
