using UnityEngine;
using UnityEngine.EventSystems;
using static OVRInput;

[RequireComponent(typeof(LineRenderer))]
public class ControllerPointer : MonoBehaviour
{
    [SerializeField]
    private SetUITransformRay uiRays;
    private LineRenderer pointerLine;
    private GameObject tempPointerVals;
    public Transform gazePointerIcon;
    public GameObject HitObject;
    public Transform AppCamera;
    private GameObject DragObject;
    public GameObject Pointer;

    public GameObject PreviousHitObject = null;

    public Color InitLineColor;
    public Color TiggerColor;

    private LineRenderer lr;
    private int layer_mask;
    private void Start()
    {
        tempPointerVals = new GameObject();
        tempPointerVals.transform.parent = transform;
        tempPointerVals.name = "tempPointerVals";
        pointerLine = gameObject.GetComponent<LineRenderer>();
        pointerLine.useWorldSpace = true;

        ControllerInfo.CONTROLLER_DATA_FOR_RAYS = tempPointerVals;
        uiRays.SetUIRays();
        lr = GetComponent<LineRenderer>();
        InitLineColor = lr.material.GetColor("_TintColor");
        layer_mask = LayerMask.GetMask("Satellite", "Data", "UI");
    }

    private void LateUpdate()
    {
        if(InputManager.Instance.GetKey(InputManager.ControllerInput.btn_back))
        {
            lr.material.SetColor("_TintColor", TiggerColor);
        }
        else
        {
            lr.material.SetColor("_TintColor", InitLineColor);
        }

        Quaternion rotation = GetLocalControllerRotation(ControllerInfo.CONTROLLER);
        Vector3 position = GetLocalControllerPosition(ControllerInfo.CONTROLLER);
        Vector3 pointerOrigin = ControllerInfo.TRACKING_SPACE.position + position;
        Vector3 pointerProjectedOrientation = ControllerInfo.TRACKING_SPACE.position + (rotation * ControllerInfo.TRACKING_SPACE.forward); //(rotation * Vector3.forward);
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        Vector3 pointerDrawStart = pointerOrigin - pointerProjectedOrientation * 0.05f;
        Vector3 pointerDrawEnd = pointerDrawStart + (gazePointerIcon.transform.position - pointerDrawStart).normalized * 1.5f; //pointerOrigin + pointerProjectedOrientation * 500.0f; //gazePointerIcon.transform.position;//
        pointerLine.SetPosition(0, pointerDrawStart);
        pointerLine.SetPosition(1, pointerDrawEnd);

        tempPointerVals.transform.position = pointerDrawStart;
        tempPointerVals.transform.rotation = rotation;


        Vector3 dir = pointerDrawEnd - pointerDrawStart;
        RaycastHit hit;

        if (Physics.Raycast(pointerDrawStart, dir, out hit, Mathf.Infinity, layer_mask))
        {
           

            HitObject = hit.collider.gameObject;
            Pointer.SetActive(true);
            Pointer.transform.position = hit.point;
            //Debug.Log("Hit Object : " + HitObject.name);
            
            if (PreviousHitObject == null)
            {
                PreviousHitObject = HitObject;
                Debug.Log("Set previous hit object : " + PreviousHitObject.name);
            }
            //if hitting data
            if (HitObject.layer == 8)
            {
                //DataPoints dp = HitObject.GetComponent<DataPoints>();
                //if(InputManager.Instance.GetKey(InputManager.ControllerInput.btn_tigger))
                //{
                //    dp.FocusOnData2();
                //}
                ////dp.ShowLabel();
                //dp.OnPointerStay();

                DataInstance3D instance3D = HitObject.GetComponent<DataInstance3D>();
                instance3D.OnPointerEnter();
                if (InputManager.Instance.GetKey(InputManager.ControllerInput.btn_tigger))
                {
                    instance3D.OnMouseDownSimulated();
                }
            }

            if (HitObject.layer == 9)
            {
                ShowSatelliteInfo sInfo = HitObject.GetComponent<ShowSatelliteInfo>();
                if (InputManager.Instance.GetKeyUp(InputManager.ControllerInput.btn_tigger))
                {
                    sInfo.TurnOnOFF();
                }
                sInfo.ShowInfo();
              
            }

            if (HitObject.layer == 11)
            {
               // if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTrackedRemote))
                  //  DragObject = HitObject;
            }

            if(PreviousHitObject != HitObject)
            {
                if(PreviousHitObject.GetComponent<DataInstance3D>())
                {
                    PreviousHitObject.GetComponent<DataInstance3D>().OnPointerExit();
                }               
                PreviousHitObject = HitObject;
            }
        }
        else
        {
            HitObject = null;
            Pointer.SetActive(false);
        }
    }
}