using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatetelliteConstellation : MonoBehaviour
{
    [SerializeField]
    public static int numberOfPlains = 6;
    private static int numberOfSlots = 6;
    public GameObject tempSatellite;
    public Transform PlaneA;
    public Transform PlaneB;
    // Start is called before the first frame update
    void Start()
    {
        ConfigurePlaneAndSlots("A1", 22000);
        ConfigurePlaneAndSlots("A2", 22000);
        ConfigurePlaneAndSlots("A3", 22000);
        ConfigurePlaneAndSlots("A4", 22000);
        ConfigurePlaneAndSlots("A5", 22000);
        ConfigurePlaneAndSlots("A6", 22000);

        ConfigurePlaneAndSlots("B1", 22000);
        ConfigurePlaneAndSlots("B2", 22000);
        ConfigurePlaneAndSlots("B3", 22000);
        ConfigurePlaneAndSlots("B4", 22000);
        ConfigurePlaneAndSlots("B5", 22000);
        ConfigurePlaneAndSlots("B6", 22000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfigurePlaneAndSlots(string planeSlot, float radius)
    {
        string plane = planeSlot[0].ToString();
        int slot = int.Parse(planeSlot[1].ToString());
        radius = radius / 10000;
        float angle = slot * Mathf.PI * 2f / numberOfSlots;
        Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, transform.position.y, Mathf.Sin(angle) * radius);

        GameObject go = Instantiate(tempSatellite);

        switch (plane)
        {
            case "A":
                go.transform.parent = PlaneA;                
                break;

            case "B":
                go.transform.parent = PlaneB;
                break;

        }

        go.transform.localPosition = newPos;

    }
}
