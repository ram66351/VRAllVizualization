using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataInstance3D : MonoBehaviour
{
    
    public GameObject highlightGO;
    public TextMesh label;
    public float DelayForHide = 2;
    private float angleY;
    private Renderer[] renderer;
    private Collider[] collider;
    private Transform cam;
    private GameObject infoLabel;
    public float min = 120;
    public float max = 180;
    public delegate void SliderChanged(float min, float max);
    public static SliderChanged SliderChangedEvent;

    public Data myData;
    protected bool Visiblity = true;

    protected string info;
   
    public int MyTimeStamp;
    public double timestamp;
    
    public string Date;
    public string newDate;
    public string bDate;

    public bool isWithInTimestamp;
    // Start is called before the first frame update
    void Start()
    {
        //highlightGO.SetActive(false);
        //label.SetActive(false);
        min = 120;
        max = 180;
        cam = Camera.main.transform;
        OptimizeVisiblity(min, max);
        SliderChangedEvent += OptimizeVisiblity;
        EarthBehaviour.EnableColliderOnDataPointEvent += EnableCollider;
        TimelineManager.CheckIfMyTimeStampEvent += ShowOrHideBasedOnTimeStamp;
        //myData.time  = 

        DateTime dt = new DateTime(
        2019,
        myData.time.Month,
        myData.time.Day,
        myData.time.Hour,
        myData.time.Minute,
        myData.time.Second);
    
        timestamp = dt.Subtract(TimelineManager.baseDate).TotalSeconds;
        MyTimeStamp = Math.Abs((int)timestamp);
    }

    void OnDestroy()
    {
        SliderChangedEvent -= OptimizeVisiblity;
        EarthBehaviour.EnableColliderOnDataPointEvent -= EnableCollider;
        TimelineManager.CheckIfMyTimeStampEvent -= ShowOrHideBasedOnTimeStamp;
    }

    // Update is called once per frame
    void LateUpdate()
    {
       
    }

    public void ShowOrHideBasedOnTimeStamp()
    {
        if(TimelineManager.Instance.MaxTimeStamp > TimelineManager.Instance.MinTimeStamp)
        {
            if (MyTimeStamp > TimelineManager.Instance.MinTimeStamp && MyTimeStamp < TimelineManager.Instance.MaxTimeStamp)
            {
                isWithInTimestamp = true;
                //EnableCollider(true);
            }
            else
                isWithInTimestamp = false;
        }
        else 
        {
            if (MyTimeStamp > TimelineManager.Instance.MinTimeStamp && timestamp < TimelineManager.Instance.MaxTimeStamp)
            {
                // EnableCollider(true);
                isWithInTimestamp = true;
            }
            else if (MyTimeStamp < TimelineManager.Instance.MaxTimeStamp && timestamp > 0)
            {
                // EnableCollider(true);
                isWithInTimestamp = true;
            }
            else
                isWithInTimestamp = false;
            //EnableCollider(false);         
        }
        OptimizeVisiblity(min, max);
    }

    public void OptimisationSliderMin(float val)
    {
        min = val;
        SliderChangedEvent(min, max);
    }

    public void OptimisationSliderMax(float val)
    {
        max = val;
        SliderChangedEvent(min, max);
    }

    public virtual void OptimizeVisiblity(float min, float max)
    {
        
        angleY = Vector3.Angle(cam.forward, transform.position);

        renderer = GetComponentsInChildren<Renderer>();
        collider = GetComponentsInChildren<Collider>();

        //
        if(isWithInTimestamp)
        {
            //Visiblity = true; 
            if (angleY > min && angleY < max)
                Visiblity = true;
            else
                Visiblity = false;
            
        }
        else
        {
            Visiblity = false;
            
        }

        foreach (Renderer rend in renderer)
        {
            rend.enabled = Visiblity;
        }
        foreach (Collider col in collider)
        {
            col.enabled = Visiblity;
        }

        //PerformCustomAnim();
    }

    public virtual void PerformCustomAnim()
    {
        //Debug.Log("PerformCustomAnim in child class !");
    }

    private void OnMouseEnter()
    {
        OnPointerEnter();

    }

    private void OnMouseExit()
    {
        OnPointerExit();
    }

    private void OnMouseDown()
    {
        OnMouseDownSimulated();
    }

    public void OnMouseDownSimulated()
    {
        FocusOnData();     
    }

    public virtual void OnPointerEnter()
    {
        HighlightInstance();
        Data.Label.SetActive(true);
    }
    public void OnPointerExit()
    {
        UnHighlightInstance();
    }

    public void HighlightInstance()
    {
        highlightGO.SetActive(true);
        label.gameObject.SetActive(true);
    }

    public void UnHighlightInstance()
    {
        StartCoroutine(Unhighlight());
    }

    public void FocusOnData()
    {
        ModeBasedUI.Instance.ChangeMode(ModeBasedUI.Mode.zoom_in);
        Transform earth = InputManager.Instance.transform_Globe;
        Quaternion oldRotation = earth.rotation;
        earth.rotation = Quaternion.identity;
        Vector3 fromDirection = (transform.position - earth.position).normalized;
        Vector3 toDirection = (InputManager.Instance.transform_VRCamera.position - earth.position).normalized;
        Quaternion fromRotation = Quaternion.Inverse(Quaternion.LookRotation(fromDirection, Vector3.up));
        Quaternion targetRotation = Quaternion.LookRotation(toDirection, Vector3.up) * fromRotation;
        InputManager.Instance.ZoomIn();
        StartCoroutine(AnimateEarthRotation(0.8f, earth, oldRotation, targetRotation));
    }

    IEnumerator AnimateEarthRotation(float dt, Transform earth, Quaternion from, Quaternion to)
    {
        earth.rotation = from;
        float t = 0;
        while (t < 3)
        {        
            yield return null;
            t += Time.deltaTime / dt;
            earth.rotation = Quaternion.Lerp(from, to, t);          
        }
        SliderChangedEvent(min, max);
    }

    IEnumerator Unhighlight()
    {
        yield return new WaitForSeconds(DelayForHide);
        highlightGO.SetActive(false);
        label.gameObject.SetActive(false);
        //yield return new WaitForSeconds(2);
        //Data.Label.SetActive(false);
    }

    private void EnableCollider(bool Enable)
    {
        Collider[] collider = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider col in collider)
        {
            col.enabled = Enable;
        }
        //Renderer[] meshRend = gameObject.GetComponentsInChildren<Renderer>();
        //foreach (Renderer mr in meshRend)
        //{
        //    mr.enabled = Enable;
        //}
    }
}
