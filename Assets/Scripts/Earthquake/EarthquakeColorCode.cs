using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeColorCode : MonoBehaviour
{
    public static EarthquakeColorCode Instance;
    public Material mat_mww;
    public Material mat_mwc;
    public Material mat_mwb;
    public Material mat_mwr;
    public Material mat_ms;
    public Material mat_ml;
    public Material mat_mblg;
    public Material mat_mb;
    public Material mat_mi;
    public Material mat_me;
    public Material mat_md;
    public Material mat_mh;
    public Material mat_default;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
