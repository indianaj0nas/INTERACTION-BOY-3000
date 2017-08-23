using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour {

    public Transform lookAt;

    private bool smooth = true;
    public float smoothSpeed = 0.125f;
    private Vector3 offset = new Vector3(0.10f, 3.5f, -8f);

    private void LateUpdate()
    {
        Vector3 desiredPosition = lookAt.transform.position + offset;

        if (smooth)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
        else
        {
            transform.position = desiredPosition;
        }
       
    }
}