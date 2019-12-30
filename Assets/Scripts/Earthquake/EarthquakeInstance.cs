using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeInstance : DataInstance3D
{
    // Start is called before the first frame update

    public Material mag_mat;
    public Transform magnitudeTower;
    public GameObject go_magTower;
    public Earthquake data;
    public float dt = 1f;

    private Vector3 BoxColliderSize;
    private Vector3 BoxColliderCenter;
    private BoxCollider boxColl;
    private float d_constScale;
    private float m_constScale;

    public Color MagColor;

    public static byte transparancy = 255;

    private LineRenderer lr;

    string AdditionalInfo;

    public void Init(Data data)
    {
        myData = data;
        this.data = (Earthquake)myData;
        //ApplyColor();
        ApplyColorForSprite();

        //lr = gameObject.AddComponent<LineRenderer>();
        
        //lr.startWidth = 0.005f;
        //lr.endWidth = 0.005f;
        //lr.SetPosition(0, transform.position);
        ////Vector3 pos2 = (transform.forward - InputManager.Instance.transform_Globe.forward).normalized ;// (transform.localPosition - InputManager.Instance.transform_Globe.position).normalized;
        //Vector3 pos2 = (transform.position - InputManager.Instance.transform_Globe.position).normalized;
        //lr.SetPosition(1, transform.position + pos2/5);
        //lr.useWorldSpace = false;
        //ApplyColor();
        //
        //For Prefab 1
        //boxColl = GetComponent<BoxCollider>();
        //BoxColliderSize = boxColl.size;
        //BoxColliderCenter = boxColl.center;
        //boxColl.size = new Vector3(BoxColliderSize.x, this.data.mag / 100, BoxColliderSize.z);
        //boxColl.center = new Vector3(BoxColliderCenter.x, boxColl.size.y / 2, BoxColliderCenter.z);

        //d_constScale = depthTower.localScale.x;
        //m_constScale = magnitudeTower.localScale.x;
        //Vector3 depth = new Vector3(d_constScale, -(d_constScale + this.data.depth / 2000.0f), d_constScale);
        //Vector3 mag = new Vector3(m_constScale, m_constScale + this.data.mag * 1.5f, m_constScale);
        //depthTower.localScale = depth;
        //magnitudeTower.localScale = mag;
        //StartCoroutine(AnimateScale(dt));

        //for Prefab 2
        d_constScale = magnitudeTower.localScale.y;
        float scale = 0; //this.data.mag / 4;
        Vector3 mag = new Vector3(scale, m_constScale, scale);
        magnitudeTower.localScale = mag;

        PerformCustomAnim();
        string info = "Magnitude : " + this.data.mag +"\nDepth : "+ this.data.depth;
        label.text = info;

        AdditionalInfo = "Magnitude : " + this.data.mag +
                                "\nType : " + this.data.magType +
                                "\nDepth : " + this.data.depth +
                                "\nPlace : " + this.data.place;

      

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPointerEnter()
    {
        base.OnPointerEnter();
        Data.Label.GetComponent<DataLabel>().SetInfo(AdditionalInfo, transform);

    }

    public override void PerformCustomAnim()
    {
        base.PerformCustomAnim();
        if (Visiblity)
        {
            if (magnitudeTower.localScale.x == 0)
            {
                StartCoroutine(AnimateScale(dt));
            }
        }
        else
        {
            magnitudeTower.localScale = new Vector3(m_constScale, 0, m_constScale);
        }
    }

    IEnumerator AnimateScale(float time)
    {
            
        Vector3 depth = new Vector3(d_constScale, -(d_constScale + data.depth / 2000.0f), d_constScale);
        float newScale = m_constScale + data.mag / 4;
        Vector3 mag = new Vector3(newScale, newScale, newScale);

        float t = 0;
        float MagVal = 0;
        string info = "";

        while (t < 3)
        {
            yield return null;
            t += Time.deltaTime / time;
            magnitudeTower.localScale = Vector3.Lerp(magnitudeTower.localScale, mag, t);
            MagVal = Mathf.Lerp(0, data.mag, t);
            label.text = MagVal.ToString();
        }
    }

    void ApplyColor()
    {
        if (data.magType.ToLower() == "mww")
        {
            mag_mat = EarthquakeColorCode.Instance.mat_mww;
        }
        else if (data.magType.ToLower() == "mwc")
        {
            mag_mat = EarthquakeColorCode.Instance.mat_mwc;
        }
        else if (data.magType.ToLower() == "mwb")
        {
            mag_mat = EarthquakeColorCode.Instance.mat_mwb;
        }
        else if (data.magType.ToLower() == "mwr")
        {
            mag_mat = EarthquakeColorCode.Instance.mat_mwr;
        }
        else if (data.magType.ToLower() == "ms20" || data.magType.ToLower() == "ms")
        {
            mag_mat = EarthquakeColorCode.Instance.mat_ms;
        }
        else if (data.magType.ToLower() == "ml")
        { 
            mag_mat = EarthquakeColorCode.Instance.mat_ml;
        }
        else if (data.magType.ToLower() == "mb_lg" || data.magType.ToLower() == "mlg")
        {
            mag_mat = EarthquakeColorCode.Instance.mat_mblg;
        }
        else if (data.magType.ToLower() == "md")
        {
            mag_mat = EarthquakeColorCode.Instance.mat_md;
        }
        else if (data.magType.ToLower() == "mi" || data.magType.ToLower() == "mwp")
        {
            mag_mat = EarthquakeColorCode.Instance.mat_mi;
        }
        else if (data.magType.ToLower() == "me")
        {
            mag_mat = EarthquakeColorCode.Instance.mat_me;
        }
        else if (data.magType.ToLower() == "mh")
        {
            mag_mat = EarthquakeColorCode.Instance.mat_mh;
        }
        else if (data.magType.ToLower() == "mb")
        {
            mag_mat = EarthquakeColorCode.Instance.mat_mb;
        }
        else
        {
            mag_mat = EarthquakeColorCode.Instance.mat_default;
        }

        //go_magTower.GetComponentInChildren<Renderer>().material = mag_mat; // SetColor("_Color", Color.green); 

        //o_magTower.GetComponent<Sprite>().color = mag_mat
        lr.material = mag_mat;
    }

     void ApplyColorForSprite()
    {
        if(data.magType.ToLower() == "mww")
        {
            MagColor = new Color32(31,209, 30, transparancy);           
        }
        else if(data.magType.ToLower() == "mwc")
        {
            MagColor = new Color32(14, 209, 96, transparancy);           
        }
        else if (data.magType.ToLower() == "mwb")
        {
            MagColor = new Color32(14, 209, 192, transparancy);       
        }
        else if (data.magType.ToLower() == "mwr")
        {
            MagColor = new Color32(14, 131, 209, transparancy);            
        }
        else if (data.magType.ToLower() == "ms20" || data.magType.ToLower() == "ms")
        {
            MagColor = new Color32(14, 47, 209, transparancy);            
        }
        else if (data.magType.ToLower() == "ml")
        {
            MagColor = new Color32(66, 14, 209, transparancy);           
        }
        else if (data.magType.ToLower() == "mb_lg" || data.magType.ToLower() == "mlg")
        {
            MagColor = new Color32(135, 14, 209, transparancy);          
        }
        else if (data.magType.ToLower() == "md")
        {
            MagColor = new Color32(209, 14, 200, transparancy);           
        }
        else if (data.magType.ToLower() == "mi" || data.magType.ToLower() == "mwp")
        {
            MagColor = new Color32(209, 14, 101, transparancy);   
        }
        else if (data.magType.ToLower() == "me")
        {
            MagColor = new Color32(209, 23,    14, transparancy);           
        }
        else if (data.magType.ToLower() == "mh")
        {
            MagColor = new Color32(209, 107, 14, transparancy);         
        }
        else if(data.magType.ToLower() == "mb")
        {
            MagColor = new Color32(205, 255, 0, transparancy);         
        }
        else
        {
            MagColor = new Color32(204, 209, 13, transparancy);           
        }

        go_magTower.GetComponent<SpriteRenderer>().color = MagColor;
    }
}
