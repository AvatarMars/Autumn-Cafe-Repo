using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityStandardAssets.Characters.FirstPerson;

public class PickUpAndInteract : MonoBehaviour
{
    public FirstPersonController fpsController;

    public Transform cam;
    public Transform camReturnPosition;

    public LayerMask canPickup;
    public LayerMask canPlace;
    public LayerMask InteractableMask;
    public LayerMask customerMask;

    public bool canInteract;

    public TMP_Text tooltipText;
    public bool showingInteractible;
    public bool toolTipsEnabled;

    public Transform dest;
    public Transform inspectDest;
    public float pickupDistance;
    public float interactDistance;

    public GameObject heldItem;
    public bool Inspecting;
    public TMP_Text inspectTip;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (toolTipsEnabled)
        {
            RaycastHit hitInteractible;
            if (Physics.Raycast(cam.position, cam.forward, out hitInteractible, pickupDistance, InteractableMask))
            {
                if (hitInteractible.collider.gameObject.GetComponent<Interactible>() && canInteract && !heldItem)
                {
                    tooltipText.text = "F:" + hitInteractible.collider.gameObject.GetComponent<Interactible>().tooltipText;
                    tooltipText.color = new Color32(205, 65, 212, 200);
                    showingInteractible = true;
                }
                else if (hitInteractible.collider.gameObject.GetComponent<Interactible>() && canInteract && heldItem && hitInteractible.collider.gameObject.GetComponent<Interactible>().interactableWithHeldItem)
                {
                    tooltipText.text = "F:" + hitInteractible.collider.gameObject.GetComponent<Interactible>().tooltipText;
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
                RaycastHit raycastHit;
                if (Physics.Raycast(cam.position, cam.forward, out toolHit, pickupDistance, customerMask))
                {
                    var customer = toolHit.collider.GetComponent<Customer>();
                    if (customer)
                    {
                        if (heldItem.GetComponent<Meal>())
                        {
                            tooltipText.text = $"E: Give {heldItem.GetComponent<Pickupable>().tooltipText} To {customer.Name}";
                            tooltipText.color = new Color32(221, 119, 93, 200);
                        }
                    }
                }
                else if (Physics.Raycast(cam.position, cam.forward, out raycastHit, pickupDistance, canPickup))
                {
                    if (raycastHit.collider.tag == "Plate")
                    {
                        tooltipText.text = "E: Put " + heldItem.GetComponent<Pickupable>().tooltipText + " On " + raycastHit.collider.GetComponent<Plate>().toolTipText;
                        tooltipText.color = new Color32(221, 119, 93, 200);
                    }
                    else if (raycastHit.collider.tag == "Bowl")
                    {
                        tooltipText.text = "E: Put " + heldItem.GetComponent<Pickupable>().tooltipText + " In " + raycastHit.collider.GetComponent<Plate>().toolTipText;
                        tooltipText.color = new Color32(221, 119, 93, 200);
                    }
                }
                else if (Physics.Raycast(cam.position, cam.forward, out toolHit, pickupDistance, canPlace))
                {
                    if (heldItem.GetComponent<mixingBowl>() && toolHit.collider.GetComponent<Mixer>())
                    {
                        tooltipText.text = "E: Put " + heldItem.GetComponent<Pickupable>().tooltipText + " In " + toolHit.collider.GetComponent<Mixer>().toolTipText;
                        tooltipText.color = new Color32(221, 119, 93, 200);
                    }
                    else if (toolHit.normal == new Vector3(0, 1, 0))
                    {
                        tooltipText.text = "E: Place " + heldItem.GetComponent<Pickupable>().tooltipText;
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
                if (Physics.Raycast(cam.position, cam.forward, out toolHit, pickupDistance, customerMask))
                {
                    var customer = toolHit.collider.GetComponent<Customer>();
                    if (customer)
                    {
                        tooltipText.text = $"E: Talk To {customer.Name}";
                        tooltipText.color = new Color32(221, 119, 93, 200);
                    }
                }
                else if (Physics.Raycast(cam.position, cam.forward, out toolHit, pickupDistance, canPickup))
                {
                    if (toolHit.collider.gameObject.GetComponent<Pickupable>())
                    {
                        tooltipText.text = "E: Pick Up " + toolHit.collider.gameObject.GetComponent<Pickupable>().tooltipText;
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
                if (hit.collider.gameObject.GetComponent<Interactible>() && hit.collider.gameObject.GetComponent<Interactible>().interactableWithHeldItem == true)
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
            else if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, customerMask) && hit.collider.GetComponent<Customer>())
            {
                ManageCustomerInteraction(hit.collider.GetComponent<Customer>());
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && heldItem && canInteract)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, customerMask) && hit.collider.GetComponent<Customer>())
            {
                ManageCustomerInteraction(hit.collider.GetComponent<Customer>());
            }
            else if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPickup) && hit.collider.GetComponent<Plate>())
            {
                PlaceOnPlate(hit.collider.gameObject);
            }
            else if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPlace) && hit.collider.gameObject.GetComponent<Mixer>() && heldItem.GetComponent<mixingBowl>())
            {
                PlaceOnMixer(hit.collider.gameObject.GetComponent<Mixer>());
            }
            else if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPlace) && hit.collider.gameObject.GetComponent<CuttingBoard>() && heldItem.GetComponent<Cuttable>())
            {
                PlaceOnBoard(hit.collider.gameObject.GetComponent<CuttingBoard>());
            }
            else if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPlace) && hit.collider.gameObject.GetComponent<BakingSlot>() && heldItem.GetComponent<Bakeable>())
            {
                PlaceInOven(hit.collider.gameObject.GetComponent<BakingSlot>());
            }
            else if (Physics.Raycast(cam.position, cam.forward, out hit, pickupDistance, canPlace))
            {
                if (hit.normal == new Vector3(0, 1, 0))
                {
                    Place(hit.point);
                }
            }
        }

        if (heldItem)
        {
            if (heldItem.GetComponent<Pickupable>().canBeInspected == true)
            {
                if (toolTipsEnabled == true)
                {
                    inspectTip.gameObject.SetActive(true);
                    if (Inspecting)
                    {
                        inspectTip.text = "X: Stop Inspecting";
                    }
                    else
                    {
                        inspectTip.text = "X: Inspect";
                    }
                }
                else
                {
                    inspectTip.gameObject.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    Inspect();
                }

                if (Inspecting)
                {
                    Vector3 vertRotAxis = heldItem.transform.InverseTransformDirection(cam.TransformDirection(Vector3.right)).normalized;
                    Vector3 hortRotAxis = heldItem.transform.InverseTransformDirection(cam.TransformDirection(Vector3.up)).normalized;
                    float horizontalRot = Input.GetAxis("Horizontal") * -100 * Time.deltaTime;
                    float verticalRot = Input.GetAxis("Vertical") * 100 * Time.deltaTime;
                    heldItem.transform.Rotate(vertRotAxis, verticalRot);
                    heldItem.transform.Rotate(hortRotAxis, horizontalRot);
                }
            }
            else
            {
                inspectTip.gameObject.SetActive(false);
            }
        }
        else
        {
            inspectTip.gameObject.SetActive(false);
        }
    }


    public void Grab()
    {
        if (heldItem.transform.parent && heldItem.transform.parent.GetComponent<Plate>())
        {
            heldItem.transform.parent.GetComponent<Plate>().onPlate.Remove(heldItem);
        }
        if (heldItem.transform.parent && heldItem.transform.parent.GetComponent<Mixer>())
        {
            heldItem.transform.parent.GetComponent<Mixer>().Bowl = null;
            heldItem.GetComponent<mixingBowl>().mixAmount = 0;
        }
        heldItem.GetComponent<Collider>().enabled = false;
        heldItem.GetComponent<Collider>().isTrigger = false;
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
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
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

    public void PlaceOnPlate(GameObject plate)
    {
        heldItem.transform.parent = plate.transform;
        heldItem.GetComponent<Rigidbody>().isKinematic = true;
        heldItem.GetComponent<Collider>().enabled = true;
        heldItem.GetComponent<Collider>().isTrigger = true;
        plate.GetComponent<Plate>().onPlate.Add(heldItem.gameObject);
        heldItem.transform.position = plate.transform.position;
        heldItem = null;
    }

    public void PlaceOnMixer(Mixer mixer)
    {
        heldItem.transform.parent = mixer.transform;
        heldItem.GetComponent<Rigidbody>().isKinematic = true;
        heldItem.GetComponent<Collider>().enabled = true;
        heldItem.GetComponent<Collider>().isTrigger = true;
        mixer.GetComponent<Mixer>().Bowl = heldItem.GetComponent<mixingBowl>();
        heldItem.transform.position = mixer.bowlDestination.position;
        heldItem = null;
    }

    void ManageCustomerInteraction(Customer customer)
    {
        if (!customer.IsWaitingForFood) return;

        if (!customer.CanReceiveMeal && !heldItem)
        {
            customer.StartMealSelectionDialogue();
        }
        else
        {
            var meal = heldItem.GetComponent<Meal>();
            if (meal)
            {
                customer.ReceiveMeal(meal);
                heldItem = null;
            }
        }
    }

    void Inspect()
    {
        if (Inspecting)
        {
            heldItem.transform.position = dest.position;
            Inspecting = false;
            fpsController.enabled = true;
        }
        else
        {
            heldItem.transform.position = inspectDest.position;
            Inspecting = true;
            fpsController.enabled = false;
        }
    }
}
