using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    public bool interactableWithHeldItem;
    public GameObject player;

    public abstract void interactFunction();
   
}
