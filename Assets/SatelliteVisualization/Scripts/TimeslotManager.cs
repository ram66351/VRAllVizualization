using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeslotManager : MonoBehaviour
{
    public TextMesh textMesh;
    public float angle;
    private bool isInitialized = false;
    // Start is called before the first frame update
    public void Init(int number)
    {
        isInitialized = true;
        textMesh.text = number + "";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isInitialized)
        {
            Vector3 vec1 = (transform.position - InputManager.Instance.transform_Globe.position).normalized;
            Vector3 vec2 = InputManager.Instance.transform_VRCamera.forward;
       
            angle = Mathf.Abs(Vector3.Angle(vec1, vec2));
            if (angle < 90)
                angle = 0;
            Color col = textMesh.color;
            col.a = AppUtils.Remap(angle, 0, 180, 0, 1) - (180-angle)/100.0f;
            textMesh.color = col;

           
        }
    }
}
