using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public Transform transform_VRCamera;
    public Transform transform_Globe;
    public float ZoomMin = -0.7f;
    public float ZoomMax = -1.0f;
    public GameObject InitScreen;
    public GameObject AnimationWindow;


    public enum ControllerInput
    {
        btn_back, btn_touchpad, btn_tigger, mouse
    }

    public List<int> CurrentKeys = new List<int>();
    public List<int> DownKeys = new List<int>();
    public List<int> UpKeys = new List<int>();

    private int totalInput;
    // Start is called before the first frame update

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        totalInput = Enum.GetNames(typeof(ControllerInput)).Length;
        AnimationWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        UpKeys.Clear();
        if (!GetKey(ControllerInput.btn_touchpad) && CurrentKeys.Contains((int)ControllerInput.btn_touchpad))
        {
            UpKeys.Add((int)ControllerInput.btn_touchpad);
        }

        if (!GetKey(ControllerInput.mouse) &&  CurrentKeys.Contains((int)ControllerInput.mouse))
        {
            UpKeys.Add((int)ControllerInput.mouse);
        }

        if (!GetKey(ControllerInput.btn_back) && CurrentKeys.Contains((int)ControllerInput.btn_back))
        {
            UpKeys.Add((int)ControllerInput.btn_back);
        }

        if (!GetKey(ControllerInput.btn_tigger) && CurrentKeys.Contains((int)ControllerInput.btn_tigger))
        {
            UpKeys.Add((int)ControllerInput.btn_tigger);
        }

        DownKeys.Clear();
        if (GetKey(ControllerInput.btn_touchpad) && !CurrentKeys.Contains((int)ControllerInput.btn_touchpad))
        {
            DownKeys.Add((int)ControllerInput.btn_touchpad);
        }

        if (GetKey(ControllerInput.mouse) && !CurrentKeys.Contains((int)ControllerInput.mouse))
        {
            DownKeys.Add((int)ControllerInput.mouse);
        }

        if (GetKey(ControllerInput.btn_back) && !CurrentKeys.Contains((int)ControllerInput.btn_back))
        {
            DownKeys.Add((int)ControllerInput.btn_back);
        }

        if (GetKey(ControllerInput.btn_tigger) && !CurrentKeys.Contains((int)ControllerInput.btn_tigger))
        {
            DownKeys.Add((int)ControllerInput.btn_tigger);
        }

        CurrentKeys.Clear();
        if(GetKey(ControllerInput.btn_touchpad))
        {
            CurrentKeys.Add((int)ControllerInput.btn_touchpad);
        }

        if (GetKey(ControllerInput.mouse))
        {
            CurrentKeys.Add((int)ControllerInput.mouse);
        }

        if (GetKey(ControllerInput.btn_back))
        {
            CurrentKeys.Add((int)ControllerInput.btn_back);
        }

        if (GetKey(ControllerInput.btn_tigger))
        {
            CurrentKeys.Add((int)ControllerInput.btn_tigger);
        }
    }

    public bool GetKey(ControllerInput input)
    {
        if ((ControllerInput) input == ControllerInput.btn_back)
        {
            return OVRInput.Get(OVRInput.Button.Back, OVRInput.Controller.RTrackedRemote);
        }
        else if ((ControllerInput)input == ControllerInput.btn_touchpad)
        {
            return OVRInput.Get(OVRInput.Button.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote);
        }
        else if ((ControllerInput)input == ControllerInput.btn_tigger)
        {
            return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTrackedRemote);
        }
        else if ((ControllerInput)input == ControllerInput.mouse)
        {
            return Input.GetMouseButton(0);
        }
        else
            return false;

    }

    public bool GetKeyDown(ControllerInput input)
    {
        if(DownKeys.Contains((int) input))
        {
            return true;
        }

        return false;
    }

    public bool GetKeyUp(ControllerInput input)
    {
        if (UpKeys.Contains((int)input))
        {
            return true;
        }

        return false;
    }

    public void ZoomIn()
    {
        Debug.Log("Zoom in");
        StartCoroutine(CameraZoom(0.8f, ZoomMax));
    }

    public void ZoomOut()
    {
        StartCoroutine(CameraZoom(0.8f, ZoomMin));
    }

    IEnumerator CameraZoom(float dt, float zoomVal)
    {
        float t = 0;
        Vector3 zoomPos = new Vector3(transform_Globe.position.x, transform_Globe.position.y, zoomVal);
        while (t < 3)
        {
            yield return null;
            t += Time.deltaTime / dt;
            transform_Globe.position = Vector3.Lerp(transform_Globe.position, zoomPos, t);
        }
    }

    public void DisableInitScreen()
    {
        InitScreen.SetActive(false);

    }
}
