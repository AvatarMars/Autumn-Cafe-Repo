using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAndInteract : MonoBehaviour
{
    public Transform cam;
    public Transform camReturnPosition;

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


        if (Input.GetKeyDown(KeyCode.E) && !heldItem && canInteract)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPickup) && hit.collider.gameObject.GetComponent<CuttingBoard>())
            {
                heldItem = hit.collider.gameObject;
                hit.collider.gameObject.GetComponent<CuttingBoard>().onBoard = null;
                Grab();
            }
            else if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPickup) && !hit.collider.gameObject.GetComponent<CuttingBoard>())
            {
                heldItem = hit.collider.gameObject;
                Grab();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && heldItem && canInteract)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPlace) && hit.collider.gameObject.GetComponent<CuttingBoard>() && heldItem.GetComponent<Cuttable>())
            {
                PlaceOnBoard(hit.collider.gameObject.GetComponent<CuttingBoard>());
            }
            else if(Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPlace) && !hit.collider.gameObject.GetComponent<CuttingBoard>())
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

    void PlaceOnBoard(CuttingBoard board)
    {
        heldItem.transform.parent = null;
        heldItem.transform.position = board.cuttingPosition.position;
        heldItem.transform.rotation = board.cuttingPosition.rotation;
        heldItem.GetComponent<Collider>().enabled = true;
        board.onBoard = heldItem;
        heldItem = null;
    }
}
