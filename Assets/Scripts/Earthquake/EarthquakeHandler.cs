using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public SortedDictionary<int, Earthquake> earthquakeDict = new SortedDictionary<int, Earthquake>();
    // Start is called before the first frame update
    void Start()
    {
        CSVLoader.CSVLoaderThread(earthquakeData.text, ReceiveData);
    }

    void ReceiveData(string[,] data)
    {
        earthquakeGrid = data;
        Debug.Log("Received Data ! ");
        isDataReceived = true;
    }

    void LateUpdate()
    {
        if (isDataReceived)
        {
            PopulateData();
           
        }
    }

    void PopulateData()
    {
        row = earthquakeGrid.GetUpperBound(0);
        col = earthquakeGrid.GetUpperBound(1);

        //earthquakeParent = new GameObject();
        //earthquakeParent.transform.position = transform.position;
        //earthquakeParent.AddComponent<MeshRenderer>();
        //earthquakeParent.AddComponent<MeshFilter>();
        //earthquakeParent.transform.parent = transform;

        for (int i = 1; i < col; i++)
        {
            float lat;
            float lon;
            float mag;
            string magType;

            if (!float.TryParse(earthquakeGrid[1, i], out lat))
            {

                Debug.Log("Latit,ude Invalid ! at postiton [" + 1 + "," + i + "], the value is : " + lat);
                continue;
            }

            if (!float.TryParse(earthquakeGrid[2, i], out lon))
            {
                Debug.Log("Latitude Invalid ! at postiton [" + 2 + "," + i + "]");
                continue;
            }

            if (!float.TryParse(earthquakeGrid[4, i], out mag))
            {
                Debug.Log("Magnitude Invalid ! at postiton [" + 4 + "," + i + "]");
                continue;
            }

            DateTime dt = DateTime.Now;
            if (!DateTime.TryParse(earthquakeGrid[0, i], out dt))
            {
                Debug.Log("Wrong Date Format : " + earthquakeGrid[0, i]);
            }

            //float _lat, float _lon, DateTime _time, float _mag, string _magType, GameObject _prefab, GameObject _label)
            Earthquake quake = new Earthquake(lat, lon, dt, mag, earthquakeGrid[5, i], prefab, label);
            quake.CreateMyInstance(i, ()=> { quake.Instance3D.transform.SetParent(transform, false); });

            earthquakeDict.Add(i, quake);
            //Vector3 dataPos = AppUtils.LatLonToPositionOnSphere(lat, lon, transform.localScale.x/2.0f);
            //GameObject DataPoint = Instantiate(prefab, dataPos, Quaternion.identity, earthquakeParent) as GameObject;

            //Vector3 dir = new Vector3(0, 1, 0);
            //Vector3 crossDir = Vector3.Cross(dir, dataPos);
            //float angle = Vector3.Angle(dir, dataPos);
            //DataPoint.transform.Rotate(crossDir, angle, Space.Self);
            //float constScale = DataPoint.transform.localScale.x;
        }

        isDataReceived = false;

       //StartCoroutine(CombineMesh(earthquakeParent.transform));


    }
    // Update is called once per frame
    void Update()
    {
        
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
        yield return null;
    }
}
