using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialKnifeController : MonoBehaviour
{
    [System.NonSerialized]
    public Transform cuttingItem;
    public GameObject knife;

    public Animator anim;

    bool cutting;
    bool collidingWithBoard;

    private void OnEnable()
    {
        transform.position = new Vector3( cuttingItem.position.x,cuttingItem.position.y + .4f, cuttingItem.position.z);
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) && cutting == false)
        {
            transform.Rotate(new Vector3(0,0, 1f));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0,0, -1f));
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cut();
            cutting = true;
            anim.SetBool("chopping", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cutting = false;
            anim.SetBool("chopping", false);
        }
    }

    void Cut()
    {
        
    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
