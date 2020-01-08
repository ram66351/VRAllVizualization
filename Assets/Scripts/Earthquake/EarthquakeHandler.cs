using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class EarthquakeHandler : MonoBehaviour
{
    
    public TextAsset earthquakeData;
    public string[,] earthquakeGrid;
    public bool isDataReceived = false;
    public int row;
    public int col;
    public GameObject prefab;
    public GameObject label;
    public Transform earthquakeParent;
    public SortedDictionary<string, Earthquake> earthquakeDict = new SortedDictionary<string, Earthquake>();
    // Start is called before the first frame update

        [SerializeField]
    public ParticleSystem.Particle[] earthquakeParticle;
    public ParticleSystem quakePS;
    public bool bPointsUpdated = false;

    public bool ShowData = true;
    public Sprite ShowIcon;
    public Sprite HideIcon;
    public Button Btn_ShowOrHideData;

    void Start()
    {
        Btn_ShowOrHideData.GetComponent<Image>().sprite = ShowIcon;
        Btn_ShowOrHideData.onClick.AddListener(ShowOrHideVizualization);
        CSVLoader.CSVLoaderThread(earthquakeData.text, ReceiveData);
    }

    void ReceiveData(string[,] data)
    {
        Debug.Log("Received Data ! ");
        earthquakeGrid = data;
        isDataReceived = true;
    }

    void LateUpdate()
    {
        if (isDataReceived)
        {
            PopulateData(earthquakeGrid);
            isDataReceived = false;
        }
    }

    void PopulateData(string[,] grid)
    {
        row = grid.GetUpperBound(0);
        col = grid.GetUpperBound(1);

        for (int i = 1; i < col; i++)
        {
            float lat;
            float lon;
            float mag;
            string magType;
            float depth;

            if (!float.TryParse(grid[1, i], out lat))
            {

                Debug.Log("Latitude Invalid ! at postiton [" + 1 + "," + i + "], the value is : " + lat);
                continue;
            }

            if (!float.TryParse(grid[2, i], out lon))
            {
                Debug.Log("Latitude Invalid ! at postiton [" + 2 + "," + i + "]");
                continue;
            }

            if (!float.TryParse(grid[4, i], out mag))
            {
                Debug.Log("Magnitude Invalid ! at postiton [" + 4 + "," + i + "]");
                continue;
            }

            if (!float.TryParse(grid[3, i], out depth))
            {
                Debug.Log("Magnitude Invalid ! at postiton [" + 3 + "," + i + "]");
                continue;
            }

            DateTime dt = DateTime.Now;
            if (!DateTime.TryParse(grid[0, i], out dt))
            {
                Debug.Log("Wrong Date Format : " + grid[0, i]);
            }

            //float _lat, float _lon, DateTime _time, float _mag, string _magType, GameObject _prefab, GameObject _label)

            double timestamp = dt.Subtract(ModeBasedUI.baseDate).TotalSeconds;
            Earthquake quake = new Earthquake(lat, lon, dt, mag, grid[5, i], prefab, label);
            quake.CreateMyInstance(i, ()=> { quake.Instance3D.transform.SetParent(earthquakeParent, false); });
            quake.depth = depth;
            quake.id = grid[11, i];
            if (!earthquakeDict.ContainsKey(quake.id))
                earthquakeDict.Add(quake.id, quake);
            else
            {
                Debug.Log("Key already existing : " + quake.id);
            }

            //Vector3 dataPos = AppUtils.LatLonToPositionOnSphere(lat, lon, transform.localScale.x /2);
            //earthquakeParticle[i].position = dataPos;

            //epos[i] = dataPos;
            //Vector3 dataPos = AppUtils.LatLonToPositionOnSphere(lat, lon, transform.localScale.x/2.0f);
            //GameObject DataPoint = Instantiate(prefab, dataPos, Quaternion.identity, earthquakeParent) as GameObject;

            //Vector3 dir = new Vector3(0, 1, 0);
            //Vector3 crossDir = Vector3.Cross(dir, dataPos);
            //float angle = Vector3.Angle(dir, dataPos);
            //DataPoint.transform.Rotate(crossDir, angle, Space.Self);
            //float constScale = DataPoint.transform.localScale.x;
        }

        bPointsUpdated = true;

        TimelineManager.Instance.BroadcastEvent();
        //PointCloud.Instance.SetPoints(epos);
        //TestParticle.Instance.SetParticlePos(epos);
        //StartCoroutine(CombineMesh(earthquakeParent.transform));

    }
   

    IEnumerator CombineMesh(Transform parent)
    {
        
        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        parent.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        parent.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        parent.transform.gameObject.SetActive(true);

        //parent.transform.localScale = new Ve
        yield return null;
    }

    public void ShowOrHideVizualization()
    {
        ShowData = !ShowData;
        if(ShowData)
        {
            Btn_ShowOrHideData.GetComponent<Image>().sprite = ShowIcon;
        }
        else
        {
            Btn_ShowOrHideData.GetComponent<Image>().sprite = HideIcon;
        }

        foreach (KeyValuePair<string, Earthquake> entry in earthquakeDict)
        {
            entry.Value.Instance3D.SetActive(ShowData);
        }

    }
}
