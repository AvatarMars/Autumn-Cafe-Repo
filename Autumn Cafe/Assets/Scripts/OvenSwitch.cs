using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenSwitch : Interactible
{
    public GameObject Body;
    public GameObject Door;
    public OvenDoor doorScript;
    public GameObject DoorPivot;
    public Oven oven;

    public bool on;

    public override void interactFunction()
    {
        if (!oven.open)
        {
            if (on)
            {
                oven.on = false;
                on = false;
            }
            else
            {
                oven.on = true;
                on = true;
            }
        }
    }
}
