using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingSlot : Interactible
{
    public Oven oven;
    public Collider colliderComponent;

    public GameObject occupant;

    public override void interactFunction()
    {
        if (player.GetComponent<PickUpAndInteract>().heldItem)
        {
            if (occupant)
            {
                //do nothing
            }
            else
            {
                if (player.GetComponent<PickUpAndInteract>().heldItem.GetComponent<Bakeable>())
                {
                    player.GetComponent<PickUpAndInteract>().PlaceInOven(this);
                }
            }
        }
        else
        {
            if (occupant)
            {
                player.GetComponent<PickUpAndInteract>().heldItem = occupant;
                if (occupant.GetComponent<Bakeable>())
                {
                    occupant.GetComponent<Bakeable>().slotOccupying = null;
                }
                occupant = null;
                player.GetComponent<PickUpAndInteract>().Grab();
            }
            else
            {
                //do nothing
            }
        }
    }
}
