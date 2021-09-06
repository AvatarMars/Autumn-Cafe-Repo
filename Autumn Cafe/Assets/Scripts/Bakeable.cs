using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bakeable : MonoBehaviour
{
    public BakingSlot slotOccupying;
    public float bakeAmount;
    public float targetBakeAmount;
    public GameObject product;

    private void Update()
    {
        if(bakeAmount >= targetBakeAmount)
        {
            if (product)
            {
                GameObject Instance = Instantiate(product, null);
                Instance.transform.position = transform.position;
                Instance.transform.rotation = transform.rotation;
                Instance.GetComponent<Rigidbody>().isKinematic = true;
                Instance.GetComponent<Collider>().enabled = true;

                if (Instance.GetComponent<Bakeable>())
                {
                    Instance.GetComponent<Bakeable>().slotOccupying = slotOccupying;
                }

                if (transform.parent && transform.parent.GetComponent<Plate>())
                {
                    Instance.transform.parent = transform.parent;
                    transform.parent.GetComponent<Plate>().onPlate.Add(Instance);
                    Instance.GetComponent<Collider>().isTrigger = true;
                }
                else
                {
                    slotOccupying.occupant = Instance;
                }
                Destroy(gameObject);
            }
        }
    }
}
