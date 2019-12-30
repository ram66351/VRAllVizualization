using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppModeManager : MonoBehaviour
{
    public enum AppMode
    {
        rotate, time
    }

    public AppMode appMode;

    public Button btn_rotate;
    public Button btn_time;

    public static AppModeManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        btn_rotate.onClick.AddListener(RotateModeOn);
        btn_time.onClick.AddListener(TimeModeOn);
        RotateModeOn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateModeOn()
    {
        appMode = AppMode.rotate;
        btn_rotate.GetComponent<Image>().color = Color.green;
        btn_time.GetComponent<Image>().color = Color.white;

    }

    public void TimeModeOn()
    {
        appMode = AppMode.time;
        btn_rotate.GetComponent<Image>().color = Color.white;
        btn_time.GetComponent<Image>().color = Color.green;
    }
}
