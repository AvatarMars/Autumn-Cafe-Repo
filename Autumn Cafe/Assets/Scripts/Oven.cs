using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    public GameObject Body;
    public OvenSwitch Switch;
    public GameObject Door;
    public OvenDoor doorScript;
    public GameObject DoorPivot;
    public GameObject Light;
    public List<BakingSlot> bakingPositions = new List<BakingSlot>();

    public bool open;
    public bool on;
    public float bakeSpeed;

    private void Update()
    {
        if (on)
        {
            Light.SetActive(true);

            foreach(BakingSlot s in bakingPositions)
            {
                if(s.occupant != null)
                {
                    s.occupant.GetComponent<Bakeable>().bakeAmount += bakeSpeed;
                }
            }
        }
        else
        {
            Light.SetActive(false);
        }
    }
}

