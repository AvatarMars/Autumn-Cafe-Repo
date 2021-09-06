using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class CuttingBoard : Interactible
{
    public bool isCutting;

    public Transform cameraPosition;
    public Transform cuttingPosition;
    public Transform knifeRestingPos;
    public GameObject knife;
    public GameObject walls;
    public GameObject canvas;
    public Image healthBar;

    public GameObject onBoard;
    public float value;

    public override void interactFunction()
    {
        player.GetComponent<PickUpAndInteract>().canInteract = false;
        player.GetComponent<PickUpAndInteract>().tooltipText.gameObject.SetActive(false);
        player.GetComponent<FirstPersonController>().enabled = false;
        player.GetComponentInChildren<Camera>().gameObject.transform.position = cameraPosition.position;
        player.GetComponentInChildren<Camera>().gameObject.transform.rotation = cameraPosition.rotation;
        isCutting = true;

        if (onBoard)
        {
            if (onBoard.GetComponent<Cuttable>().radial)
            {
                enableRadialKnife();
                walls.SetActive(true);
            }
            else
            {
                enableFreeKnife();
                walls.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isCutting)
        {
            stopInteracting();
            walls.SetActive(false);
        }

        if(knife.GetComponent<KnifeController>().enabled && onBoard != null)
        {
            canvas.SetActive(true);
            healthBar.fillAmount = onBoard.GetComponent<Cuttable>().health / onBoard.GetComponent<Cuttable>().maxHealth;
        }
        else
        {
            canvas.SetActive(false);
        }
    }

    void stopInteracting()
    {
        player.GetComponent<PickUpAndInteract>().canInteract = true;
        player.GetComponent<PickUpAndInteract>().tooltipText.gameObject.SetActive(true);
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
