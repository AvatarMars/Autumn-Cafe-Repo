using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform cam;

    public LayerMask canPickup;
    public LayerMask canPlace;

    public bool canInteract;
    public LayerMask InteracableMask;

    public Transform dest;
    public float pickupDistance;

    public GameObject heldItem;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F) && !heldItem && canInteract)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, InteracableMask))
            {
                hit.collider.gameObject.GetComponent<Interactible>().interactFunction();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F) && heldItem && canInteract)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, InteracableMask))
            {
                if(hit.collider.gameObject.GetComponent<Interactible>().interactableWithHeldItem == true)
                {
                    hit.collider.gameObject.GetComponent<Interactible>().interactFunction();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.E) && !heldItem)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPickup))
            {
                heldItem = hit.collider.gameObject;
                Grab();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && heldItem)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPlace))
            {
                Place(hit.point);
            }
        }
    }


    void Grab()
    {
        heldItem.GetComponent<Collider>().enabled = false;
        heldItem.GetComponent<Rigidbody>().isKinematic = true;
        heldItem.transform.position = dest.position;
        heldItem.transform.parent = dest.gameObject.transform;
    }

    void Place(Vector3 placePos)
    {
        heldItem.transform.parent = null;
        heldItem.transform.position = placePos;
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem.GetComponent<Collider>().enabled = true;
        heldItem = null;
    }
}
