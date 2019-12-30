using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataPoints : MonoBehaviour
{

    public GameObject Label;
    public float distance;
    public float factor = 10;
    //private Color initColor;
    

    private Renderer renderer;
    [SerializeField]
    private TextMesh infoLabel;
    public GameObject Quad;
    [SerializeField]
    private GameObject QuadHighlighter;
    [SerializeField]
    private Texture ArrowTex;
    [SerializeField]
    private Texture DefaultTex;
    private Material QuadMat;
    private Storm storm;
    private StormColorCodeCategory stromCategory;

    public static DataPoints _instance;
    public static DataPoints Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DataPoints>();
            }

            return _instance;
        }
    }

    public delegate void ApplyArrowTextureOnZoom(bool appy);
    public static ApplyArrowTextureOnZoom applyArrowTextureEvent;

    public bool IsClicked = false;
    public static Transform CommonParent;
    // Start is called before the first frame update
    public void InitLabel(Data data, StormColorCodeCategory _s)
    {
        if (data is Storm)
        {
            storm = (Storm)data;
            stromCategory = _s;
            string info = "Name : " + storm.name + ", \n " +
                          "Basin : " + storm.basin + ", \n " +
                          "Sub-basin : " + storm.subBasin;

            EarthBehaviour.EnableColliderOnDataPointEvent += EnableCollider;
            infoLabel.text = info;
            QuadMat = Quad.GetComponent<MeshRenderer>().material;
            DefaultTex = QuadMat.mainTexture;
            infoLabel.gameObject.SetActive(false);
            QuadHighlighter.SetActive(false);
            applyArrowTextureEvent += ApplyArrowTexture;
        }
       
    }

    void OnDestroy()
    {
        EarthBehaviour.EnableColliderOnDataPointEvent -= EnableCollider;
        applyArrowTextureEvent -= ApplyArrowTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowLabel()
    {
        //distance = Vector3.Distance(transform.position, Camera.main.transform.position);   
        //Vector3 pos = transform.position + (Camera.main.transform.position - transform.position) / 2;
        //Label.transform.position = new Vector3(pos.x,  -pos.y, pos.z); //transform.position * 1.5f;
        Label.SetActive(true);
        //Label.GetComponent<DataLabel>().SetInfo(storm, stromCategory);
        Label.GetComponent<DataLabel>().from = transform;
       // Label.transform.LookAt(Camera.main.transform);
        //Label.transform.localScale = new Vector3(distance, distance, distance)/factor;
        
    }

    public void HideLabel()
    {
        //StartCoroutine(WaitForAWhile());
        Label.SetActive(false);
    }

    IEnumerator WaitForAWhile()
    {
        yield return new WaitForSeconds(3);
        Label.SetActive(false);
    }

    private void OnMouseEnter()
    {
        OnPointerStay();
        
    }

    private void OnMouseExit()
    {
        OnPointerExit();
        
    }

    private void OnMouseDown()
    {
        FocusOnData2();
        HighlightTheStorm();
        DataSelected();
    }

    public void OnPointerStay()
    {
        infoLabel.gameObject.SetActive(true);
        QuadHighlighter.SetActive(true);
        ShowLabel();
        HighlightTheStorm();
    }
    public void OnPointerExit()
    {
        infoLabel.gameObject.SetActive(false);
        QuadHighlighter.SetActive(false);
        HideLabel();
        UnHighlightStorm();
    }

    public void ApplyArrowTexture(bool apply)
    {
        if(apply)
        {
            QuadMat.mainTexture = ArrowTex;
        }
        else
        {
            QuadMat.mainTexture = DefaultTex;
        }
    }

    public void FocusOnData()
    {
        Transform VRcam = InputManager.Instance.transform_VRCamera;
        Vector3 Dir = transform.position - InputManager.Instance.transform_Globe.position;
        Dir = Dir.normalized;
        //VRcam.position = transform.position + Dir/5;
        // VRcam.LookAt(transform.parent);
       
        StartCoroutine(AnimateCamRotation(0.15f, VRcam, transform.position + Dir / 2));
    }

    public void FocusOnData2()
    {
        //HighlightTheStorm();
        ModeBasedUI.Instance.ChangeMode(ModeBasedUI.Mode.zoom_in);
        Transform earth = InputManager.Instance.transform_Globe;
        Quaternion oldRotation = earth.rotation;
        earth.rotation = Quaternion.identity;
        Vector3 fromDirection = (transform.position - earth.position).normalized;
        Vector3 toDirection = (InputManager.Instance.transform_VRCamera.position - earth.position).normalized;
        Quaternion fromRotation = Quaternion.Inverse(Quaternion.LookRotation(fromDirection, Vector3.up));
        Quaternion targetRotation = Quaternion.LookRotation(toDirection, Vector3.up) * fromRotation;
        //earth.rotation = targetRotation;
        InputManager.Instance.ZoomIn();

        //Debug.Log(fromRotation +" "+ targetRotation +" earth forward : "+ earth.forward);

        StartCoroutine(AnimateEarthRotation(0.8f, earth, oldRotation, targetRotation));
        applyArrowTextureEvent(true);
        DataSelected();

        //Transform earth = InputManager.Instance.transform_Globe;
        //Vector3 fromDirection = transform.position - earth.position;
        //Vector3 toDirection = InputManager.Instance.transform_VRCamera.position - earth.position;
        //earth.rotation *= Quaternion.FromToRotation(fromDirection, toDirection);

        ////r_parent.transform.LookAt(InputManager.Instance.transform_VRCamera.position);
        // r_parent.transform.rotation = Quaternion.LookRotation(transform.position - InputManager.Instance.transform_VRCamera.position);
        //Quaternion rot = Quaternion.LookRotation(transform.position - InputManager.Instance.transform_VRCamera.position);
        //StartCoroutine(AnimateEarthRotation(0.5f, earth, rot));
       
    }

    private void EnableCollider(bool Enable)
    {
        Collider[] collider = gameObject.GetComponentsInChildren<Collider>();
        foreach(Collider col in collider)
        {
            col.enabled = Enable;
        }
        MeshRenderer[] meshRend = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in meshRend)
        {
            mr.enabled = Enable;
        }
    }

    IEnumerator AnimateCamRotation(float dt, Transform cam, Vector3 to)
    {
        float t = 0;
        float MagVal = 0;
        string info = ""; 

        while (t < 3)
        {
            yield return null;
            t += Time.deltaTime / dt;
            cam.position = Vector3.Lerp(cam.position, to, t);
            cam.LookAt(transform.parent);
        }   
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

        ShowAnimationWindow();
    }

    private Gradient StormActualGradient;
    private Gradient gradient;
    private LineRenderer lr;

    private bool isHighlighted = false;
    void HighlightTheStorm()
    {
        CommonParent = transform.parent;
        if (StormActualGradient == null)
        {
            lr = transform.parent.gameObject.GetComponent<LineRenderer>();
            StormActualGradient = lr.colorGradient;
            
        }
        else
        {
            //Debug.Log("Parent Name : " + transform.parent.gameObject.name + ", gradient color :" + StormActualGradient.colorKeys.Length);
        }
       
        if(gradient == null)
        {
           if(!isHighlighted)
            {
                gradient = new Gradient();
                GradientColorKey[] colorKey = new GradientColorKey[8];
                GradientAlphaKey[] alphaKey = new GradientAlphaKey[8];

                for (int i = 0; i < 8; i++)
                {
                    Color col = new Color(Mathf.Abs(1 - StormActualGradient.colorKeys[i].color.r),
                                          Mathf.Abs(1 - StormActualGradient.colorKeys[i].color.g),
                                          Mathf.Abs(1 - StormActualGradient.colorKeys[i].color.b), 1.0f);
                    colorKey[i] = new GradientColorKey(col, i / 8);
                    alphaKey[i] = new GradientAlphaKey(1.0f, i / 8);
                }

                gradient.SetKeys(
                    colorKey,
                     alphaKey
                   );
                isHighlighted = true;
            }
        }

        lr.colorGradient = gradient;
    }

    void UnHighlightStorm()
    {
            isHighlighted = false;
        StartCoroutine(ApplyDefaultColor());
    }


    void DataSelected()
    {
        IsClicked = !IsClicked;
        if (IsClicked)
            lr.colorGradient = gradient;
        else
            lr.colorGradient = StormActualGradient;
    }

    IEnumerator ApplyDefaultColor()
    {
        yield return new WaitForSeconds(2f);
        if(!isHighlighted)
        {
            if (StormActualGradient != null)
            {
                transform.parent.gameObject.GetComponent<LineRenderer>().colorGradient = StormActualGradient;
            }
        }        
    }

    virtual public void ShowAnimationWindow()
    {
        Debug.Log("ShowAnimationWindow : MEthod called in parent");
    }
}
