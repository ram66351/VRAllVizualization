using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

using System;

public class StormDataHandler : MonoBehaviour
{
    public TextAsset StormData;
    public int DataSplit = 10;
    public int col;
    public int row;
    public GameObject StromPrefab;
    public GameObject Globe;
    public Material LineMat;
    public GameObject Label;

    private string[,] StormGrid;
    private float Radius;

    private bool isDataReceived = false;
    public SortedDictionary<string, List<Storm>> StormDictionary = new SortedDictionary<string, List<Storm>>();

    public static StormDataHandler Instance;

    public bool ShowData = true;
    public Sprite ShowIcon;
    public Sprite HideIcon;
    public Button Btn_ShowOrHideData;

    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        Btn_ShowOrHideData.GetComponent<Image>().sprite = ShowIcon;
        Btn_ShowOrHideData.onClick.AddListener(ShowOrHideVizualization);
        Radius = Globe.transform.localScale.x/2;
        CSVLoader.CSVLoaderThread(StormData.text, ReceiveData);
    }

    void ReceiveData(string[,] data)
    {
        StormGrid = data;
        Debug.Log("Received Data ! ");
        isDataReceived = true;
    }

    void PopulateData()
    {
        row = StormGrid.GetUpperBound(0);
        col = StormGrid.GetUpperBound(1);

        for (int i = 1; i < col; i+=2)
        {
            float lat, lon = 0;
            if (!float.TryParse(StormGrid[8, i], out lat))
            {
                continue;
            }

            if (!float.TryParse(StormGrid[9, i], out lon))
            {
                continue;
            }

            DateTime dt = DateTime.Now;
            if (!DateTime.TryParse(StormGrid[6, i], out dt))
            {
                Debug.Log("Wrong Date Format : " + StormGrid[5, i]);
            }

            Storm strom = new Storm(lat, lon, StromPrefab, dt, Label);
          
            strom.serialNo = StormGrid[0, i];
            strom.season = StormGrid[1, i];
            strom.number = StormGrid[2, i]; 
            strom.basin = StormGrid[3, i];
            strom.subBasin = StormGrid[4, i];
            strom.name = StormGrid[5, i];

      
            strom.nature = StormGrid[7, i];
            strom.wind = StormGrid[10, i];
            strom.pressure = StormGrid[11, i];
            strom.windPercentile = StormGrid[12, i];
            strom.pressurePercentile = StormGrid[13, i];
            strom.trackType = StormGrid[14, i];

            double timeStamp = System.DateTime.Now.Subtract(strom.time).TotalSeconds;
            strom.convertedTimestamp = timeStamp;

            if(StormDictionary.ContainsKey(strom.serialNo))
            {
                StormDictionary[strom.serialNo].Add(strom);
            }
            else
            {
                List<Storm> stromList = new List<Storm>();
                stromList.Add(strom);
                StormDictionary.Add(strom.serialNo, stromList);
            }
        }

        isDataReceived = false;
        int a = 0;
        foreach (KeyValuePair<string, List<Storm>> entry in StormDictionary)
        {
            a++;
            
            List<Storm> myStormList = entry.Value;
            GameObject StormMaster = new GameObject();
            StormMaster.transform.SetParent(transform);
            StormMaster.name = entry.Value[0].name;
            StormMaster.transform.position = myStormList[myStormList.Count / 2].posOnSphere * 1.1f;
            LineRenderer lr = StormMaster.AddComponent<LineRenderer>();
            lr.positionCount = myStormList.Count;
            lr.useWorldSpace = false;
            lr.startWidth = 0.03f;
            lr.endWidth = 0.03f;
            lr.material = LineMat;

            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            GradientColorKey[] colorKey = new GradientColorKey[8];
            GradientAlphaKey[] alphaKey = new GradientAlphaKey[8];
            
            GameObject prevStormPt = null;

            int inner_count = 0;
            int Split = 1;
            int limit = 1;

            if(myStormList.Count < 8)
            {
                Split = myStormList.Count;
                limit = Split;
            }
            else
            {
                Split = myStormList.Count / 8;
                limit = 8;
            }
          
            for (int j =0; j< myStormList.Count; j++)
            {
                lr.SetPosition(j, myStormList[j].posOnSphere);

                //GameObject colliderPoint = Instantiate(StromPrefab, myStormList[j].posOnSphere, Quaternion.identity);
                //colliderPoint.name = myStormList[j].name + "_" + j;
                //colliderPoint.transform.SetParent(StormMaster.transform, false);
                //myStormList[j].gameObject = colliderPoint;
                //colliderPoint.GetComponent<StromDataPoint>().Init(myStormList[j], Label);

                //Vector3 dir = new Vector3(0, 1, 0);
                //Vector3 crossDir = Vector3.Cross(dir, colliderPoint.transform.position);
                //float angle = Vector3.Angle(dir, colliderPoint.transform.position);
                //colliderPoint.transform.Rotate(crossDir, angle, Space.Self);
              
                myStormList[j].CreateMyInstance(j, () => { myStormList[j].Instance3D.transform.SetParent(StormMaster.transform, false); });
                
                if ( j%Split == 0 && inner_count < limit)
                {
                    float wind = float.Parse(myStormList[j].wind);
                    Color col = StormColorCode.Instance.GetColorCode(wind);
                    colorKey[inner_count] = new GradientColorKey(col, inner_count/8);
                    alphaKey[inner_count] = new GradientAlphaKey(alpha, inner_count / 8);
                    inner_count++;
                }
               

                if (j == 0)
                {
                    prevStormPt = myStormList[j].Instance3D;
                }
                else
                {
                    prevStormPt.GetComponent<StormInstance>().FaceNextPoint(myStormList[j].Instance3D.transform);
                    prevStormPt = myStormList[j].Instance3D;
                }
            }

            gradient.SetKeys(
             colorKey,
              alphaKey
            );

            lr.colorGradient = gradient;
            // do something with entry.Value or entry.Key
        }
    }

    

    // Update is called once per frame
   

    void LateUpdate()
    {
        if(isDataReceived)
        {
            PopulateData();
        }
    }

    public void ShowOrHideVizualization()
    {
        ShowData = !ShowData;
        if (ShowData)
        {
            Btn_ShowOrHideData.GetComponent<Image>().sprite = ShowIcon;
        }
        else
        {
            Btn_ShowOrHideData.GetComponent<Image>().sprite = HideIcon;
        }

        foreach (KeyValuePair<string, List<Storm>> entry in StormDictionary)
        {
            entry.Value[0].Instance3D.transform.parent.gameObject.SetActive(ShowData);
            //for (int j = 0; j < entry.Value.Count; j++)
            //{
            //    entry.Value[j].Instance3D.SetActive(ShowData);
            //}
        }

    }
}
