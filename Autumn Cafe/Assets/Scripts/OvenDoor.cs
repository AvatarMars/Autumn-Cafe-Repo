using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenDoor : Interactible
{
    public Oven oven;
    public GameObject Body;
    public GameObject DoorPivot;
    public Animator DoorPivotAnim;

    public bool open;

    public override void interactFunction()
    {
        MoveDoor();
    }

    void MoveDoor()
    {
        if (open)
        {
            DoorPivotAnim.SetBool("Open", false);
            open = false;
            oven.open = false;
        }
        else
        {
            DoorPivotAnim.SetBool("Open", true);
            open = true;
            oven.open = true;
            if(oven.on == true)
            {
                oven.on = false;
            }
        }
    }
}
