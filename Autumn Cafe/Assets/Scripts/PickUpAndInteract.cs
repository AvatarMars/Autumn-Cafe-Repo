using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpAndInteract : MonoBehaviour
{
    public Transform cam;
    public Transform camReturnPosition;

    public LayerMask canPickup;
    public LayerMask canPlace;

    public bool canInteract;
    public LayerMask InteractableMask;

    public TMP_Text tooltipText;
    public bool showingInteractible;
    public bool toolTipsEnabled;

    public Transform dest;
    public float pickupDistance;
    public float interactDistance;

    public GameObject heldItem;

    private void Update()
    {
        if (toolTipsEnabled)
        {
            RaycastHit hitInteractible;
            if (Physics.Raycast(cam.position, cam.forward, out hitInteractible, pickupDistance, InteractableMask))
            {
                if (hitInteractible.collider.gameObject.GetComponent<Interactible>() && canInteract && !heldItem)
                {
                    tooltipText.text = hitInteractible.collider.gameObject.GetComponent<Interactible>().tooltipText;
                    tooltipText.color = new Color32(205, 65, 212, 200);
                    showingInteractible = true;
                }
                else if (hitInteractible.collider.gameObject.GetComponent<Interactible>() && canInteract && heldItem && hitInteractible.collider.gameObject.GetComponent<Interactible>().interactableWithHeldItem)
                {
                    tooltipText.text = hitInteractible.collider.gameObject.GetComponent<Interactible>().tooltipText;
                    tooltipText.color = new Color32(205, 65, 212, 200);
                    showingInteractible = true;
                }
            }
            else
            {
                showingInteractible = false;
                tooltipText.text = "";
                tooltipText.color = new Color32(255, 255, 255, 0);
            }
        }
        

        if (showingInteractible == false && toolTipsEnabled)
        {
            if (heldItem)
            {
                RaycastHit toolHit;
                if (Physics.Raycast(cam.position, cam.forward, out toolHit, pickupDistance, canPlace))
                {
                    if(toolHit.normal == new Vector3(0, 1, 0))
                    {
                        tooltipText.text = "Place " + heldItem.GetComponent<Pickupable>().tooltipText;
                        tooltipText.color = new Color32(221, 119, 93, 200);
                    }
                }
                else
                {
                    tooltipText.text = "";
                    tooltipText.color = new Color32(255, 255, 255, 0);
                }
            }
            else
            {
                RaycastHit toolHit;
                if (Physics.Raycast(cam.position, cam.forward, out toolHit, pickupDistance, canPickup))
                {
                    if (toolHit.collider.gameObject.GetComponent<Pickupable>())
                    {
                        tooltipText.text = "Pick Up " + toolHit.collider.gameObject.GetComponent<Pickupable>().tooltipText;
                        tooltipText.color = new Color32(221, 119, 93, 200);
                    }
                }
                else
                {
                    tooltipText.text = "";
                    tooltipText.color = new Color32(255, 255, 255, 0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && !heldItem && canInteract)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, interactDistance, InteractableMask))
            {
                if (hit.collider.gameObject.GetComponent<Interactible>())
                {
                    hit.collider.gameObject.GetComponent<Interactible>().interactFunction();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.F) && heldItem && canInteract)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, interactDistance, InteractableMask))
            {
                if(hit.collider.gameObject.GetComponent<Interactible>() && hit.collider.gameObject.GetComponent<Interactible>().interactableWithHeldItem == true)
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
            else if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPickup) && hit.collider.gameObject.GetComponent<Bakeable>())
            {
                heldItem = hit.collider.gameObject;
                if (heldItem.GetComponent<Bakeable>().slotOccupying)
                {
                    heldItem.GetComponent<Bakeable>().slotOccupying.occupant = null;
                    heldItem.GetComponent<Bakeable>().slotOccupying = null;
                }
                Grab();
            }
            else if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, InteractableMask) && hit.collider.gameObject.GetComponent<BakingSlot>())
            {
                if (hit.collider.gameObject.GetComponent<BakingSlot>().occupant)
                {
                    heldItem = hit.collider.gameObject.GetComponent<BakingSlot>().occupant;
                    heldItem.GetComponent<Bakeable>().slotOccupying.occupant = null;
                    heldItem.GetComponent<Bakeable>().slotOccupying = null;
                }
                Grab();
            }
            else if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPickup))
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
            else if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPlace) && hit.collider.gameObject.GetComponent<BakingSlot>() && heldItem.GetComponent<Bakeable>())
            {
                PlaceInOven(hit.collider.gameObject.GetComponent<BakingSlot>());
            }
            else if(Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPlace))
            {
                if (hit.normal == new Vector3(0, 1, 0))
                {
                    Place(hit.point);
                }
            }
        }
    }


    public void Grab()
    {
        heldItem.GetComponent<Collider>().enabled = false;
        heldItem.GetComponent<Rigidbody>().isKinematic = true;
        heldItem.transform.position = dest.position;
        heldItem.transform.parent = dest.gameObject.transform;
    }

    public void Place(Vector3 placePos)
    {
        heldItem.transform.parent = null;
        heldItem.transform.position = placePos;
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem.GetComponent<Collider>().enabled = true;
        heldItem = null;
    }

    public void PlaceOnBoard(CuttingBoard board)
    {
        heldItem.transform.parent = null;
        heldItem.transform.position = board.cuttingPosition.position;
        heldItem.transform.rotation = board.cuttingPosition.rotation;
        heldItem.GetComponent<Collider>().enabled = true;
        board.onBoard = heldItem;
        heldItem = null;
    }

    public void PlaceInOven(BakingSlot slot)
    {
        heldItem.transform.parent = null;
        heldItem.transform.position = slot.transform.position;
        heldItem.transform.rotation = slot.transform.rotation;
        heldItem.GetComponent<Collider>().enabled = true;
        slot.occupant = heldItem;
        heldItem.GetComponent<Bakeable>().slotOccupying = slot;
        heldItem = null;
    }
}
