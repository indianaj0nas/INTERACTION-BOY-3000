using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Movement : MonoBehaviour {


    public float walkSpeed;
    public float runSpeed;
    public float groundDistance;
    public float jumpForce;
    public float maxJumpHeight = 2f;
    public float minJumpHeight = .5f;
    public float dashDistance = 5f;
    public float timeToJumpApex;
    public float accelerationTimeGrounded;
    public float accelerationTimeAirborne;
    public int pressedShift;
    public float turnSpeed;
    public LayerMask Ground;
    public Vector3 drag;

    private float gravity;
    private float speed;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private Vector3 velocitySmoothing;
    private Vector3 _velocity;
    private CharacterController _controller;
    private Transform _groundChecker;
    private bool _isGrounded = true;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _groundChecker = transform.Find("GroundChecker");

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update()
    {
        Movement();
        Gravity();
        Jump();
        DashDrag();
    }

    private void Movement()
    {
        if (Input.GetButton("Run"))
        {
            speed = runSpeed;
            // turnSpeed = 0.05f;
        }
        else if (!Input.GetButtonUp("Run"))
        {
            speed = walkSpeed;
            //turnSpeed = 0.5f;
        }

        float deadzone = 0.5f;
        Vector3 stickInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (stickInput.magnitude < deadzone)
            stickInput = Vector3.zero;
        else
            stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));

        Vector3 targetVelocity = stickInput * speed;
        _velocity = Vector3.SmoothDamp(_velocity, targetVelocity, ref velocitySmoothing, (_isGrounded) ? accelerationTimeGrounded : accelerationTimeAirborne);
        _controller.Move(stickInput * Time.deltaTime * speed);

        if (stickInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(stickInput, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
      

    }

    void Gravity()
    {
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        _isGrounded = Physics.CheckSphere(_groundChecker.position, groundDistance, Ground, QueryTriggerInteraction.Ignore);
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = 0f;

        //gravity = (2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);


    }

    void Jump()
    {
        /* if (Input.GetButtonDown("Jump") && _isGrounded)
             _velocity.y += Mathf.Sqrt(jumpHeight * 2f * gravity);*/

        if (Input.GetButtonDown("Jump") && _isGrounded)
            _velocity.y = maxJumpVelocity;

        if (Input.GetButtonUp("Jump"))
        {
            if (_velocity.y > minJumpVelocity)
                _velocity.y = minJumpVelocity;
        }

        //gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);

    }

    void DashDrag()
    {
        if (Input.GetButtonDown("Run"))
        {
            Debug.Log("dash");
            _velocity += Vector3.Scale(transform.forward, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime),
                0, 
                (Mathf.Log(1f / (Time.deltaTime * drag.z + 1)) / -Time.deltaTime)));
        }

        _velocity.x /= 1 + drag.x * Time.deltaTime;
        _velocity.y /= 1 + drag.y * Time.deltaTime;
        _velocity.z /= 1 + drag.z * Time.deltaTime;
    }
}
