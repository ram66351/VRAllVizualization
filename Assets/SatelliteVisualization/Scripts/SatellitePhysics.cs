using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SatellitePhysics : MonoBehaviour
{
    public TextMesh SatelliteName;
    public string name;
    public float a;
    public float b;
    public float c;
    public float Inclination;
    [SerializeField] float alpha;
    [SerializeField] float deltaAlpha;
    public Vector3 center;
    public Transform focus1;
    [SerializeField] Transform model;


    float r;

    public LineRenderer lr;
    
    [Range(3, 50)]
    public int segments;
    public Orbit orbit;

    public float orbitProgress;
    public float orbitPeriod;
    public bool orbitActive = true;

    private Vector3 OrbitalRotation;
    private int index;
    private int orbitalPlane = 6;

    public Transform SpotLight;

    public SatelliteObject satelliteObject;
    public static float speedConstant = 0.5f;
    public static bool isPaused = false;

    public float testSpeed = 1.0f;
    void Awake()
    {
        
    }

    //void Start()
    //{
    //    lr = GetComponent<LineRenderer>();
    //    if (a > b)
    //    {
    //        c = Mathf.Sqrt((a * a) - (b * b));
    //        center = new Vector3(focus1.position.x + c, 0, focus1.position.z);
    //    }
    //    else
    //    {
    //        c = Mathf.Sqrt((b * b) - (a * a));
    //        center = new Vector3(focus1.position.x, 0, focus1.position.z + c);
    //    }
    //    orbit = new Orbit(a, b, center);
    //    DrawOrbit();
    //    SetOrbitalPosition();
    //    StartCoroutine(AnimateOrbit());
    //}


    public void Init(string _name, float perigee, float apogee, float inclination, float period, Transform focus, int index , SatelliteObject obj)
    {
        this.name = _name; 
        this.a = perigee;
        this.b = apogee;
        this.Inclination = inclination;
        this.orbitPeriod = period;
        this.focus1 = focus;
        this.index = index;
        this.satelliteObject = obj;
        lr = GetComponent<LineRenderer>();
        if(a > b)
        {
            c = Mathf.Sqrt((a * a) - (b * b));
            center = new Vector3(focus1.position.x + c, 0, focus1.position.z);
        }     
        else
        {
            c = Mathf.Sqrt((b * b) - (a * a));
            center = new Vector3(focus1.position.x, 0, focus1.position.z + c);
        }
        orbit = new Orbit(a, b, center);

        SatelliteName.text = name;
        DrawOrbit();
          
        //transform.SetParent(focus1);
        SetOrbitalPosition();
        //StartCoroutine(AnimateOrbit());
        
        SpotLight.GetComponent<Light>().color = StormColorCode.Instance.Mat[Random.Range(0, 12)].GetColor("_TintColor");
    }

    void DrawOrbit()
    { 
       
        Vector3[] points = new Vector3[segments + 1];
        for (int i = 0; i < segments; i++)
        {
            Vector2 pos2D = orbit.Evaluate((float)i / (float)segments);
            points[i] = new Vector3(pos2D.x, 0, pos2D.y);
        }
        points[segments] = points[0];
        lr.positionCount = segments + 1;
        lr.SetPositions(points);


       // transform.eulerAngles = Vector3.zero; //GetOrbitalPlane() * Inclination;

    }

    void Update()
    {
        //transform.position = new Vector3(center.x + a * Mathf.Cos(alpha), 0, center.z + b * Mathf.Sin(alpha));

        //orbit.MoveAroundOrbit(model, alpha);
        //alpha += deltaAlpha;

        SpotLight.LookAt(focus1);

        if (OVRInput.Get(OVRInput.Button.Back, OVRInput.Controller.RTrackedRemote) || Input.GetMouseButtonUp(0))
        {
            isPaused = !isPaused;
        }

        if (SatelliteHandler.UniversalPause)
        {
            if(SatelliteHandler.Instance.gestureMode == SatelliteHandler.GestureMode.Freeze)
            {
                if (InputManager.Instance.GetKey(InputManager.ControllerInput.btn_touchpad) || Input.mouseScrollDelta.y != 0)
                {
                    float val = 0;

#if UNITY_EDITOR
                    val = Input.mouseScrollDelta.y/4.0f;
#else
                    Quaternion q = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
                    val = q.z / 50.0f;
#endif

                    //transform.Rotate(0, val * freezeMovementSpeed * Time.deltaTime, 0);
                    float orbitSpeed = 1 / orbitPeriod;
                    orbitProgress += Time.deltaTime * val * RotateEarth.freezeMovementSpeed;
                    orbitProgress %= 1;
                    //SetOrbitalPosition();
                    // Debug.Log("Val : " + val + ", Orbit progress : " + orbitProgress);
                    Vector2 OrbitPosition = orbit.Evaluate(orbitProgress);
                    Vector3 newPos = new Vector3(OrbitPosition.x, 0, OrbitPosition.y);
                    model.localPosition = newPos; //Vector3.Lerp(model.localPosition,  newPos, 0.5f* Time.deltaTime);
                    SpotLight.LookAt(focus1);
                }
            }
        }
        else
        {
            if (orbitPeriod < 0.1f)
            {
                orbitPeriod = 0.1f;
            }
            float orbitSpeed = 1 / orbitPeriod;
            orbitProgress += Time.deltaTime * orbitSpeed * speedConstant;
            orbitProgress %= 1;
            SetOrbitalPosition();
            SpotLight.LookAt(focus1);
        }

    }

    void SetOrbitalPosition()
    {
        Vector2 OrbitPosition = orbit.Evaluate(orbitProgress);
        model.localPosition = new Vector3(OrbitPosition.x, 0, OrbitPosition.y);
    }

    

    IEnumerator AnimateOrbit()
    {
        if(orbitPeriod < 0.1f)
        {
            orbitPeriod = 0.1f;
        }
        float orbitSpeed = 1 / orbitPeriod;

        while(orbitActive)
        {
            orbitProgress += Time.deltaTime * orbitSpeed * speedConstant;
            orbitProgress %= 1;
            SetOrbitalPosition();
            yield return  null;
        }
    }

    public Vector3 GetOrbitalPlane()
    {
        int plane = index % orbitalPlane;
        if (plane == 0)
        {
            return new Vector3(1, 0, 0);
        }
        else if (plane == 1)
        {
            return new Vector3(-1, 0, 0);
        }
        else if(plane == 2)
        {
            return new Vector3(0, 1, 0);
        }
        else if(plane == 3)
        {
            return new Vector3(0, -1, 0);
        }
        else if(plane == 4)
        {
            return new Vector3(0, 0, 1);
        }
        else
        {
            return new Vector3(0, 0, -1);
        }
    }

}