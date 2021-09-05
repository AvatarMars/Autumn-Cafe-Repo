using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cuttable;

public class KnifeController : MonoBehaviour
{

    public Transform cuttingItem;
    public GameObject knife;
    public Transform blade;

    public Animator anim;

    public LayerMask planeLayer;

    bool cutting;

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
            anim.SetBool("chopping", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("chopping", false);
        }
    }

    private void OnDisable()
    {
        knife.transform.localPosition = new Vector3(0.37f,0,0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Cut()
    {
        RaycastHit cutHit;
        if (Physics.Raycast(blade.position, Vector3.down, out cutHit, 3f))
        {
            if (cutHit.collider.GetComponent<Cuttable>())
            {
                cutHit.collider.GetComponent<Cuttable>().health--;
                if (cutHit.collider.GetComponent<Cuttable>().health <= 0)
                {
                    foreach (SpawnableEntry s in cutHit.collider.GetComponent<Cuttable>().Spawnables)
                    {
                        GameObject Instance = Instantiate(s.spawnable, cutHit.collider.transform.position, Quaternion.Euler(0, s.yRotationOffset + transform.localEulerAngles.y, 0));

                        if(Instance.GetComponent<Cuttable>())
                        {
                            transform.root.GetComponent<CuttingBoard>().onBoard = Instance;
                        }
                        else
                        {
                            transform.root.GetComponent<CuttingBoard>().onBoard = null;
                        }
                    }
                    Destroy(cutHit.collider.gameObject);
                }
            }
        }
    }
}
