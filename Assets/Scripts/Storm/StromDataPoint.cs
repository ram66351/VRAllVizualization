using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StromDataPoint : DataPoints
{
    private Storm storm;
    private StormColorCodeCategory stromColCat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
             
    }

    public void Init(Storm _storm, GameObject _label)
    {
        storm = _storm;
        stromColCat = StormColorCode.Instance.GetColorCodeAndDescription(float.Parse(_storm.wind));
        Quad.GetComponent<Renderer>().material = stromColCat.Mat;
        Label = _label;
        InitLabel(storm, stromColCat);
        Vector3 pos = transform.localPosition;

        
        //transform.localPosition = new Vector3(pos);
    }

    public void FaceNextPoint (Transform nextPoint)
    {        
        transform.LookAt(nextPoint, transform.up);
    }

    override public void ShowAnimationWindow()
    {
        base.ShowAnimationWindow();
        Vector3 pos = transform.position;
        pos = new Vector3(pos.x, pos.y, pos.z - 0.01f);
        GameObject animWindow = InputManager.Instance.AnimationWindow;
        //animWindow.transform.position = pos;
        animWindow.SetActive(true);
        animWindow.GetComponent<AnimateCyclone>().Init(storm);
    }
}
