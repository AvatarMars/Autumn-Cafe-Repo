using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour
{
    [System.NonSerialized]
    public Transform cuttingItem;
    public GameObject knife;

    public Animator anim;

    public LayerMask planeLayer;

    bool cutting;
    bool collidingWithBoard;

    private void OnEnable()
    {
        transform.position = new Vector3(cuttingItem.position.x, cuttingItem.position.y + .4f, cuttingItem.position.z);
        transform.rotation = Quaternion.Euler(90, 0, 0);
        knife.transform.localPosition = Vector3.zero;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);       
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2, planeLayer))
        {
            if (hit.collider.tag == "MousePlane")
            {
                transform.position = new Vector3(hit.point.x,hit.point.y +.4f,hit.point.z);
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, 0, 1f));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 0, -1f));
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
        knife.transform.localPosition = new Vector3(0.37f,0,0);
    }
}
