using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera Camera;
    void Update()
    {
        if (Camera == null)
        {
            this.transform.LookAt(Camera.main.transform.position);
        }
        else
        {
            this.transform.LookAt(Camera.transform.position);
            transform.rotation = Quaternion.LookRotation(Camera.transform.forward);
        }

    }
}
