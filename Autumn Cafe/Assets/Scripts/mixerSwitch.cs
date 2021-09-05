using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mixerSwitch : Interactible
{
    public Mixer mixer;
    public bool on;

    public override void interactFunction()
    {
        if (on)
        {
            mixer.on = false;
            on = false;
        }
        else
        {
            mixer.on = true;
            on = true;
        }
    }
}
