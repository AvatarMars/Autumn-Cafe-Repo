using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixer : MonoBehaviour
{
    public string toolTipText;

    public mixingBowl Bowl;
    public Transform bowlDestination;

    public Animator mixerAnim;
    public Animator whiskAnim;

    public bool on;
    public float mixSpeed;

    [System.Serializable]
    public class MixEntry
    {
        public List<GameObject> Ingredients = new List<GameObject>();
        public float targetMixAmount;
        public GameObject product;
    }

    public List<MixEntry> allMixes = new List<MixEntry>();

    private void Update()
    {
        if (on)
        {
            Bowl.mixAmount += mixSpeed;

            foreach(MixEntry entry in allMixes)
            {
                if( Bowl.mixAmount == entry.targetMixAmount)
                {
                    bool falied = false;

                    foreach (GameObject go in entry.Ingredients)
                    {
                        foreach (GameObject obj in Bowl.onPlate)
                        {
                            if (obj.name == go.name + "(Clone)")
                            {
                                falied = false; // our ingredients match up so we havent failed yet
                                break;
                            }
                            else
                            {
                                falied = true; // there is an ingredient missing so we fail
                            }
                        }
                    }

                    if(falied == false) // we've gone through each ingredient and they all match up
                    {
                        Debug.Log("mixed!");
                        Bowl.mixAmount = 0;
                        foreach(GameObject toDelete in Bowl.onPlate)
                        {
                            Destroy(toDelete);
                        }
                        GameObject Instance = Instantiate(entry.product,Bowl.transform);
                        Instance.GetComponent<Rigidbody>().isKinematic = true;
                        Instance.GetComponent<Collider>().enabled = true;
                        Instance.GetComponent<Collider>().isTrigger = true;
                        Bowl.onPlate.Add(Instance);
                    }
                }
            }
        }
        else
        {

        }
    }
}
