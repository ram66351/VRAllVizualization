using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class AnimateCyclone : MonoBehaviour
{
    public Storm curr_storm;
    public List<Storm> storm_list;
    public Button Play;
    public Button Stop;
    public Button Close;
    public Slider slider;
    public GameObject StromEffectPrefab;
    public UILineRenderer ui_lr;
    public BezierCurve3PointLineRenderer b_Curve;
    public RectTransform Ctrl_pt_mid;
    public RectTransform Ctrl_pt_last;
    public RectTransform IndicatorLine;
    public RectTransform Animation_Line;
    public RectTransform BoxSlider;

    public float dt = 1f;
    public int CurrentCyclonePoint = 0;
    Vector3 dir = new Vector3(0, 1, 0);

    public LineRenderer curve_LR;
    public float dtt;
    public float dtt_factor = 20;
    // Start is called before the first frame update
    void Awake()
    {
        Close.onClick.AddListener(CloseWindow);


    }

    public void Init(Storm storm)
    {
        this.curr_storm = storm;
        if(StormDataHandler.Instance.StormDictionary.ContainsKey(storm.serialNo))
        {
            storm_list = StormDataHandler.Instance.StormDictionary[storm.serialNo];
            Debug.Log("Storm : " + storm.name + ", storm points : " + storm_list.Count);
            GameObject stormParent =  GameObject.Find(storm.name);
            Play.onClick.AddListener(AnimateCycloneEffect);
            Stop.onClick.AddListener(StopAnimation);

            //PlotGraph(100);
            PlotGraph2(storm_list.Count);
        }
        else
        {
            Debug.Log("Key Not Found !");
        }

        Vector3 pos = gameObject.GetComponent<RectTransform>().anchoredPosition;
        pos.x += 60;
        //gameObject.GetComponent<RectTransform>().anchoredPosition = pos;

        Vector3 crossDir = Vector3.Cross(dir, StromEffectPrefab.transform.position);
        float angle = Vector3.Angle(dir, StromEffectPrefab.transform.position);
        StromEffectPrefab.transform.Rotate(crossDir, angle, Space.Self);
       curve_LR = b_Curve.gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void AnimateCycloneEffect()
    {
        Debug.Log("AnimateCycloneEffect");
        StromEffectPrefab.transform.position = storm_list[0].Instance3D.transform.position;
        StartCoroutine(AnimCycloneEffect(0));
    }

    IEnumerator AnimCycloneEffect(int index)
    {
        CurrentCyclonePoint = index;
        Debug.Log("AnimateCycloneEffect : "+ index);
        if(index == storm_list.Count - 2)
        {
            StromEffectPrefab.transform.position = storm_list[0].Instance3D.transform.position;
            StartCoroutine(AnimCycloneEffect(0));
            
        }
        else
        {
            //yield return new WaitForSeconds(dt);
            float distance  = Vector3.Distance(storm_list[index].Instance3D.transform.position, storm_list[index + 1].Instance3D.transform.position) ;
            
            //while (distance > 0.0001f)
            {
                StromEffectPrefab.transform.position = Vector3.Lerp(StromEffectPrefab.transform.position, storm_list[index + 1].Instance3D.transform.position, 1.0f);
                StromEffectPrefab.transform.localRotation = Quaternion.Slerp(StromEffectPrefab.transform.localRotation, storm_list[index].Instance3D.transform.localRotation, 1.0f);

                float factor = 100.0f / storm_list.Count;
                Animation_Line.anchoredPosition = new Vector2(factor * index, Animation_Line.anchoredPosition.y);
            }


            if(index < curve_LR.positionCount -2)
                dtt = Mathf.Abs(curve_LR.GetPosition(index).y - curve_LR.GetPosition(index + 1).y);

            yield return new WaitForSeconds(dtt*dtt_factor + dt);

            StartCoroutine(AnimCycloneEffect(++index));
        }

    }

    public void StopAnimation()
    {
        StopCoroutine(AnimCycloneEffect(0));
    }


    public void OnTimeSliderPulled(float val)
    {
        Debug.Log(val);
        PlotGraph((int) ((1.0f - val) * 100));
    }

    public void PlotGraph(int max)
    {
        Debug.Log(max);
        Vector2[] Points = new Vector2[storm_list.Count];
        float offset = storm_list.Count / 10.0f;
        Vector3 pos = IndicatorLine.anchoredPosition;
        IndicatorLine.anchoredPosition = new Vector3(pos.x, max - 100, pos.y);
        for (int i = 0; i < Points.Length; i++)
        {
            float valX = AppUtils.Remap(i, 0, storm_list.Count, 0, 100);
            float valY = AppUtils.Remap(i, 0, storm_list.Count, 0, max);
            //Points[i] = new Vector2(offset * i, offset * i);
            Points[i] = new Vector2(valX, valY);
            //Debug.Log("i : " + i + "," + Points[i].x + "," + Points[i].y);
        }

        ui_lr.Points = Points;

        dt = AppUtils.Remap(max, 0, 100, 0.15f, 0.005f);
    }

    void PlotGraph2(int num)
    {
        b_Curve.vertexCount = num;

    }

    public void OnTimeSliderPulled2(float val)
    {
        Debug.Log(val );
        dt = AppUtils.Remap(val, 0, 1, 0.1f, 0.005f);
        //PlotGraph((int)((1.0f - val) * 100));
        Vector3 pos = Ctrl_pt_last.anchoredPosition;
        Ctrl_pt_last.anchoredPosition = new Vector3(pos.x, (int)((1.0f - val) * 100), pos.z);
        pos = Ctrl_pt_mid.anchoredPosition;
        float diff = 100 - (int)((1.0f - val) * 100);
        //Ctrl_pt_mid.anchoredPosition = new Vector3(pos.x, 50 - diff/2, pos.z);
        BoxSlider.sizeDelta = new Vector3(BoxSlider.sizeDelta.x, 100 - val * 100);
        Ctrl_pt_mid.anchoredPosition = new Vector3(0, 0, 0);
       
    }
}
