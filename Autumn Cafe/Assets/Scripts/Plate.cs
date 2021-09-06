using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public List<GameObject> onPlate = new List<GameObject>();
    public string toolTipText;

    public bool canBake;

    public virtual void Update()
    {
        onPlate.RemoveAll(GameObject => GameObject == null);

        foreach (GameObject go in onPlate)
        {
            if (go.GetComponent<Bakeable>() && canBake)
            {
                go.GetComponent<Bakeable>().bakeAmount = gameObject.GetComponent<Bakeable>().bakeAmount;
            }
        }
    }
}
