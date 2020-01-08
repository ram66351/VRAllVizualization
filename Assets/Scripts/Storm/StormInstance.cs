using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormInstance : DataInstance3D
{
    public GameObject go_StormPoint;
    private Storm storm;
    private StormColorCodeCategory stromColCat;
    private Gradient StormActualGradient;
    private Gradient gradient;
    private LineRenderer lr;
    private bool isHighlighted = false;
    //private Sprite QuadMat;
    [SerializeField]
    private Sprite ArrowTex;
    [SerializeField]
    private Sprite DefaultTex;

    public delegate void ApplyArrowTextureOnZoom(bool appy);
    public static ApplyArrowTextureOnZoom applyArrowTextureEvent;

    // Start is called before the first frame update
    public void Init(Data data)
    {
        myData = data;
        storm = (Storm)myData;
        stromColCat = StormColorCode.Instance.GetColorCodeAndDescription(float.Parse(storm.wind));
        go_StormPoint.GetComponent<SpriteRenderer>().color = stromColCat.Mat.GetColor("_TintColor");

        string info = "Name : " + storm.name + ", \n " +
              "Basin : " + storm.basin + ", \n " +
              "Sub-basin : " + storm.subBasin;

        label.text = info;
        //QuadMat = go_StormPoint.GetComponent<MeshRenderer>().material;
        DefaultTex = go_StormPoint.GetComponent<SpriteRenderer>().sprite;
        TimelineManager.CheckIfMyTimeStampEvent += ShowOrHideLineRenderer;
       // applyArrowTextureEvent += ApplyArrowTexture;

    }

    void OnDestroy()
    {
        // applyArrowTextureEvent -= ApplyArrowTexture;
        TimelineManager.CheckIfMyTimeStampEvent -= ShowOrHideLineRenderer;
    }
    

    void ShowOrHideLineRenderer()
    {
        bool enableLR = true;
        for(int i = 0; i< transform.parent.childCount; i++)
        {
            if (!transform.parent.GetChild(i).gameObject.GetComponent<DataInstance3D>().isWithInTimestamp)
            {
                enableLR = false;
                break;
            }
        }

        transform.parent.gameObject.GetComponent<LineRenderer>().enabled = enableLR;
    }

    public override void OnPointerEnter()
    {
        base.OnPointerEnter();
        HighlightTheStorm();
    }



    void HighlightTheStorm()
    {
        if (StormActualGradient == null)
        {
            lr = transform.parent.gameObject.GetComponent<LineRenderer>();
            StormActualGradient = lr.colorGradient;

        }
        else
        {
            //Debug.Log("Parent Name : " + transform.parent.gameObject.name + ", gradient color :" + StormActualGradient.colorKeys.Length);
        }

        if (gradient == null)
        {
            if (!isHighlighted)
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

    public void ApplyArrowTexture(bool apply)
    {
        if (apply)
        {
            //QuadMat.mainTexture = ArrowTex;
            go_StormPoint.GetComponent<SpriteRenderer>().sprite = ArrowTex;
        }
        else
        {
            //QuadMat.mainTexture = DefaultTex;
            go_StormPoint.GetComponent<SpriteRenderer>().sprite = DefaultTex;
        }
    }

    public void FaceNextPoint(Transform nextPoint)
    {
        transform.LookAt(nextPoint, transform.up);
    }
}
