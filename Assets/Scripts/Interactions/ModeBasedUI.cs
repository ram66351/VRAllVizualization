using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ModeBasedUI : MonoBehaviour
{
    public static ModeBasedUI Instance;
    public enum Mode
    {
        general, zoom_in
    }

    public Mode currentMode;
    public Button btn_ZoomOut;
    public static DateTime baseDate = new DateTime(1970, 01, 01);
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        btn_ZoomOut.gameObject.SetActive(false);

        btn_ZoomOut.onClick.AddListener(() => ZoomOut());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeMode(Mode mode)
    {
        currentMode = mode;
        if (mode == Mode.zoom_in)
        {
            btn_ZoomOut.gameObject.SetActive(true);
        }
        else
        {
            btn_ZoomOut.gameObject.SetActive(false);
        }
    }

    public void ZoomOut()
    {
        Debug.Log("Zoom out pressed");
        ChangeMode(Mode.general);
        InputManager.Instance.ZoomOut();
        //DataPoints.applyArrowTextureEvent(false);
    }
}
