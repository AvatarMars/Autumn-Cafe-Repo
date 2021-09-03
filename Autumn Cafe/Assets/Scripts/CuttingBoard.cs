using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CuttingBoard : Interactible
{
    public bool isCutting;

    public Transform cameraPosition;
    public Transform cuttingPosition;
    public Transform knifeRestingPos;
    public GameObject knife;

    public GameObject onBoard;

    public override void interactFunction()
    {
        player.GetComponent<PickUpAndInteract>().canInteract = false;
        player.GetComponent<FirstPersonController>().enabled = false;
        player.GetComponentInChildren<Camera>().gameObject.transform.position = cameraPosition.position;
        player.GetComponentInChildren<Camera>().gameObject.transform.rotation = cameraPosition.rotation;
        isCutting = true;

        if (onBoard.GetComponent<Cuttable>().radial)
        {
            enableRadialKnife();
        }
        else
        {
            enableFreeKnife();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isCutting)
        {
            stopInteracting();
        }
    }

    void stopInteracting()
    {
        player.GetComponent<PickUpAndInteract>().canInteract = true;
        player.GetComponent<FirstPersonController>().enabled = true;
        player.GetComponentInChildren<Camera>().gameObject.transform.position = player.GetComponent<PickUpAndInteract>().camReturnPosition.position;
        player.GetComponentInChildren<Camera>().gameObject.transform.rotation = player.GetComponent<PickUpAndInteract>().camReturnPosition.rotation;
        isCutting = false;

        knife.transform.position = knifeRestingPos.position;
        knife.GetComponent<RadialKnifeController>().enabled = false;
        knife.GetComponent<KnifeController>().enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("stop interacting");
    }

    void enableRadialKnife()
    {
        knife.GetComponent<RadialKnifeController>().cuttingItem = onBoard.transform;
        knife.GetComponent<RadialKnifeController>().enabled = true;
    }

    void enableFreeKnife()
    {
        knife.GetComponent<KnifeController>().cuttingItem = onBoard.transform;
        knife.GetComponent<KnifeController>().enabled = true;
    }
}
