using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBehaviour : MonoBehaviour
{
    public delegate void EnableColliderOnDataPoint(bool enable);
    public static EnableColliderOnDataPoint EnableColliderOnDataPointEvent;
    bool runOnce = true;
    // Start is called before the first frame update
    void Start()
    {
        Data.Earth = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //OVRInput.FixedUpdate();
        if (InputManager.Instance.GetKey(InputManager.ControllerInput.btn_touchpad))
        {
            Quaternion q = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
            //q.Set(q.x * -1, q.y * -1, q.z, q.w);
            //transform.rotation =  Quaternion.Slerp(transform.rotation, q, 1.0f * );
            //transform.eulerAngles = q.eulerAngles;
            float speed = 100.0f;
            transform.Rotate(q.x * speed * Time.deltaTime, -q.y * speed * Time.deltaTime, q.z * speed * Time.deltaTime, Space.World);

            if (runOnce)
            {
                Debug.Log("Disable collider once");
                EnableColliderOnDataPointEvent(false);
                runOnce = false;
            }
        }
        else
        {
            if (!runOnce)
            {
                Debug.Log("Enable collider once");
                EnableColliderOnDataPointEvent(true);
                runOnce = true;
            }
        }

        //if (InputManager.Instance.GetKeyUp(InputManager.ControllerInput.btn_touchpad))
        //{
        //    EnableColliderOnDataPointEvent(true);
        //    Debug.Log("Touchpad botton Up");
        //}

        //if (InputManager.Instance.GetKeyDown(InputManager.ControllerInput.btn_touchpad))
        //{
        //    EnableColliderOnDataPointEvent(false);
        //    Debug.Log("Touchpad botton Down");
        //}

        //if (InputManager.Instance.GetKeyUp(InputManager.ControllerInput.mouse))
        //{
        //    EnableColliderOnDataPointEvent(true);
        //}

        //if (InputManager.Instance.GetKeyDown(InputManager.ControllerInput.mouse))
        //{
        //    EnableColliderOnDataPointEvent(false);
        //}

    }
}
