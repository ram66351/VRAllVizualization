using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEarth : MonoBehaviour
{
    public float Speed = 5;
    public static float freezeMovementSpeed = -10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(SatelliteHandler.UniversalPause)
        {
            if (InputManager.Instance.GetKey(InputManager.ControllerInput.btn_touchpad) || Input.mouseScrollDelta.y != 0)
            {
                float val = 0;

                #if UNITY_EDITOR
                        val = Input.mouseScrollDelta.y * 4;
                #else
                        Quaternion q = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
                         val = q.z;
                #endif

                transform.Rotate(0, -val * freezeMovementSpeed * Time.deltaTime, 0);
            }
            else
            {
                transform.Rotate(0, -0.25f * Time.deltaTime, 0);
            }
        }
        else
        {
            transform.Rotate(0, Speed * Time.deltaTime, 0);
        }
        
    }
}
