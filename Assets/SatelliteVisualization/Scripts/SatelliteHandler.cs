using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SatelliteHandler : MonoBehaviour
{
    public static SatelliteHandler Instance; 
    public Transform SatelliteInfoRectParent;
    public Transform CameraRig;
    public TextAsset SatelliteData;
    public GameObject SatellitePrefab;
    public GameObject EarthGameObject;
    public GameObject PauseorPlayButton;
    public bool isDataReceived = false;
    public float ZoomSpeed = 10;
    private string[,] grid;
    private int row;
    private int col;
    [SerializeField]

    public Button btn_Rotate;
    public Button btn_Zoom;
    public Button btn_Freeze;

    public Transform PlaneA;
    public Transform PlaneB;
    public Transform PlaneC;
    public Transform PlaneD;
    public Transform PlaneE;
    public Transform PlaneF;

    public enum GestureMode
    {
        Rotate, Zoom, Freeze
    }

    public GestureMode gestureMode;

    SortedDictionary<double, SatelliteObject> SatelliteDictionary = new SortedDictionary<double, SatelliteObject>();
    public static bool UniversalPause = false;
    public static int NumberOfSlots = 6;
    // Start is called before the first frame update
    
    void Start()
    {
        Instance = this;
        CSVLoader.CSVLoaderThread(SatelliteData.text, ReceiveData);
        PasueOrPlay();
    }

    void ReceiveData(string[,] data)
    {
        grid = data;
        Debug.Log("Received Data ! ");
        isDataReceived = true;
       // PopulateData();
    }

    void LateUpdate()
    {
        if(isDataReceived)
        { 
            PopulateData();
            isDataReceived = false;
        }
    }

    void PopulateData()
    {
        row = grid.GetUpperBound(0);
        col = grid.GetUpperBound(1);

        for (int i = 1; i < col; i++)
        {
            int perigee = 0;
            int apogee = 0;
            int mass = 1600;
            int life = 777;

            if (!int.TryParse(grid[4, i], out perigee))
            {
                Debug.Log("Error in input string : " + grid[4, i]);
            }
            if (!int.TryParse(grid[5, i], out apogee))
            {
                Debug.Log("Error in input string : " + grid[5, i]);
            }
            if (!int.TryParse(grid[8, i], out mass))
            {
                Debug.Log("Error in input string : " + grid[8, i]);
            }
            if (!int.TryParse(grid[9, i], out life))
            {
                Debug.Log("Error in input string : " + grid[9, i] +", Satellite name : "+ grid[0, i]);
            }

            float inclination = float.Parse(grid[6, i]);
            float period = float.Parse(grid[7, i]);
            string planeSlot = grid[13, i].Trim();
            string plane = planeSlot[0].ToString();
            int slot = int.Parse(planeSlot[1].ToString());

            SatelliteObject satellite = new SatelliteObject(SatellitePrefab, EarthGameObject, grid[0, i], perigee, apogee, grid[3, i], inclination, period, grid[12, i],plane, slot);

            satellite.otherName = grid[1, i];
            satellite.launchTime = grid[2, i];
            satellite.rocket = grid[10, i];
            satellite.block = grid[11, i];
            satellite.prn = grid[12, i];
            satellite.life = life;
            satellite.mass = mass;
            

            if (!SatelliteDictionary.ContainsKey(satellite.TimeStamp))
            {
                SatelliteDictionary.Add(satellite.TimeStamp, satellite);
            }
            else
            {
                Debug.Log("Time stamp repeated : new " + satellite.name + ", old : " + SatelliteDictionary[satellite.TimeStamp].name);
            }
            
        }

        // SatelliteDictionary.Values.First();

        //foreach (KeyValuePair<double, SatelliteObject> entry in SatelliteDictionary)
        //{
        //    Debug.Log(entry.Value.name + " launched at " + entry.Value.LaunchAtOrbitTime);
        //}
        StartCoroutine(CreateSatellitBasedOnTime(0, SatelliteDictionary.Values.First().TimeStamp));
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        //OVRInput.FixedUpdate();
        if(gestureMode == GestureMode.Rotate)
        {
            if (InputManager.Instance.GetKey(InputManager.ControllerInput.btn_touchpad))
            {
                Quaternion q = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
                //q.Set(q.x * -1, q.y * -1, q.z, q.w);
                //transform.rotation =  Quaternion.Slerp(transform.rotation, q, 1.0f * );
                //transform.eulerAngles = q.eulerAngles;
                float speed = 100.0f;
                transform.Rotate(q.x * speed * Time.deltaTime, -q.y * speed * Time.deltaTime, q.z * speed * Time.deltaTime, Space.World);
            }
        }
        else if(gestureMode == GestureMode.Zoom)
        {
            if (InputManager.Instance.GetKey(InputManager.ControllerInput.btn_touchpad))
            {
                Quaternion q = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);

                if (CameraRig.position.z < -0.7f && CameraRig.position.z > -3.0f)
                {
                    Debug.Log("Trigger pressed !");

                }

                CameraRig.Translate(0, 0, -q.x * ZoomSpeed * Time.deltaTime);
            }
        } 
        else if(gestureMode == GestureMode.Freeze)
        {

        }
    }

    IEnumerator CreateSatellitBasedOnTime(int count, double dt)
    {
        if(count < SatelliteDictionary.Count)
        {
            SatelliteObject obj = SatelliteDictionary.ElementAt(count).Value;
            float diff = (float)(obj.TimeStamp - dt)/(10000000);
            //yield return new WaitForSeconds(diff/SatellitePhysics.speedConstant);
            yield return new WaitForSeconds(.5f);
            obj.CreateSatellite(count);
            StartCoroutine(CreateSatellitBasedOnTime(++count, obj.TimeStamp));
        }
        else
        {
            StopCoroutine(CreateSatellitBasedOnTime(count, 0));
            PasueOrPlay();
        }
       
    }

    public void PasueOrPlay()
    {
        UniversalPause = !UniversalPause;
        gestureMode = GestureMode.Freeze;
        btn_Rotate.GetComponent<Image>().color = Color.white;
        btn_Zoom.GetComponent<Image>().color = Color.white;
        btn_Freeze.GetComponent<Image>().color = Color.green;
    }

    public void EnableRotationMode()
    {
        //UniversalPause = false;
        gestureMode = GestureMode.Rotate;
        btn_Rotate.GetComponent<Image>().color = Color.green;
        btn_Zoom.GetComponent<Image>().color = Color.white;
        btn_Freeze.GetComponent<Image>().color = Color.white;
    }

    public void EnableZoomMode()
    {
       //UniversalPause = false;
        gestureMode = GestureMode.Zoom;
        btn_Rotate.GetComponent<Image>().color = Color.white;
        btn_Zoom.GetComponent<Image>().color = Color.green;
        btn_Freeze.GetComponent<Image>().color = Color.white;
    }

    public void ResetRotation()
    {
        Transform earth = InputManager.Instance.transform_Globe;
        Quaternion oldRotation = earth.rotation;
        earth.rotation = Quaternion.identity;
        Vector3 fromDirection = earth.forward;
        Vector3 toDirection = (InputManager.Instance.transform_VRCamera.position - earth.position).normalized;
        Quaternion fromRotation = Quaternion.Inverse(Quaternion.LookRotation(fromDirection, Vector3.up));
        Quaternion targetRotation = Quaternion.LookRotation(toDirection, Vector3.up) * fromRotation;
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
    }

}
