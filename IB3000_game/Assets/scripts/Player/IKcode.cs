using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKcode : MonoBehaviour {

    public float turnSpeed = 20f;
    public GameObject player;

    void Update()
    {
        transform.position = player.transform.position;

        RotateTowardsMoveDirection();
    }

    void RotateTowardsMoveDirection()
    {
        float deadzone = 0.5f;
        Vector3 stickInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (stickInput.magnitude < deadzone)
            stickInput = Vector3.zero;
        else
            stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));

       /* Vector3 targetVelocity = stickInput * speed;
        _velocity = Vector3.SmoothDamp(_velocity, targetVelocity, ref velocitySmoothing, (_isGrounded) ? accelerationTimeGrounded : accelerationTimeAirborne);*/

        if (stickInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(stickInput, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }
}
