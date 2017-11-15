using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Movement : MonoBehaviour
{


    public float walkSpeed;
    public float runSpeed;
    public float climbSpeed;
    public float groundDistance;
    public float jumpForce;
    public float maxJumpHeight = 2f;
    public float minJumpHeight = .5f;
    public float dashDistance = 5f;
    public float timeToJumpApex;
    public float accelerationTimeGrounded;
    public float accelerationTimeAirborne;
    public int pressedShift;
    public float normalTurnSpeed;
    public float otherTurnSpeed;
    public LayerMask Ground;
    public Vector3 drag;

    public float sprintDelay;
    private float turnSpeed;
    private float gravity;
    private float speed;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private Vector3 stickInput;
    private Vector3 velocitySmoothing;
    private Vector3 _velocity;
    private CharacterController _controller;
    private Transform _groundChecker;
    private Animator anim;
    private bool _isGrounded = true;
    private bool climbing;
    private bool useGravity;

    private GameObject player;
    public Transform chest;
    public Transform chestIK;
    public Transform forwardTarget;
    private Vector3 chestOffset;

    private Vector3 curNormal = Vector3.zero;
    private Vector3 usedNormal = Vector3.zero;
    private Quaternion tiltToNormal;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _groundChecker = transform.Find("GroundChecker");

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        sprintDelay = 0;
        useGravity = true;
    }

    void Update()
    {
        Movement();
        Gravity();
        Jump();
        DashDrag();
        Climbing();
    }

    private void LateUpdate()
    {
        IKtarget();
    }

    private void Movement()
    {
        if (!climbing)
        {
            if (Input.GetButton("Run"))
            {
                speed = runSpeed;
                turnSpeed = otherTurnSpeed;
            }
            else if (!Input.GetButtonUp("Run"))
            {
                speed = walkSpeed;
                turnSpeed = normalTurnSpeed;
            }

            float deadzone = 0.5f;
            Vector3 stickInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (stickInput.magnitude < deadzone)
                stickInput = Vector3.zero;
            else
                stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));

            Vector3 targetVelocity = stickInput * speed;
            _velocity = Vector3.SmoothDamp(_velocity, targetVelocity, ref velocitySmoothing, (_isGrounded) ? accelerationTimeGrounded : accelerationTimeAirborne);
            //_controller.Move(stickInput * Time.deltaTime * speed);

            if (stickInput != Vector3.zero)
            {
                _controller.Move(transform.forward * Time.deltaTime * speed);
                Quaternion targetRotation = Quaternion.LookRotation(stickInput, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            }
        }

    }

    void Gravity()
    {
        if (useGravity)
        {
            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
           // transform.Translate(_velocity * Time.deltaTime);

            _isGrounded = Physics.CheckSphere(_groundChecker.position, groundDistance, Ground, QueryTriggerInteraction.Ignore);
            if (_isGrounded && _velocity.y < 0)
                _velocity.y = 0f;

            //gravity = (2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        }
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

        if (Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + .75f, transform.position.z), .55f, 11))
        {
            _velocity.y = 0f;
        }
        //gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);

    }

    void DashDrag()
    {
        if (Input.GetButtonDown("Run"))
        {
            _velocity += Vector3.Scale(transform.forward, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime),
                0,
                (Mathf.Log(1f / (Time.deltaTime * drag.z + 1)) / -Time.deltaTime)));
        }

        _velocity.x /= 1 + drag.x * Time.deltaTime;
        _velocity.y /= 1 + drag.y * Time.deltaTime;
        _velocity.z /= 1 + drag.z * Time.deltaTime;
    }

    void IKtarget()
    {
        if (!climbing)
        {
            chest.LookAt(chestIK.position);
            chestOffset = new Vector3(0, 0, -90);
        }
        else
        {
            chestOffset = new Vector3(0, 0, 0);
        }
        
        chest.rotation = chest.rotation * Quaternion.Euler(chestOffset);

        float rotateDist = Vector3.Distance(chestIK.position, forwardTarget.position);

        /*chestIK.transform.position = (chestIK.position - forwardTarget.transform.position).normalized * 1.4f + forwardTarget.transform.position;
        chestIK.transform.position = Vector3.MoveTowards(chestIK.transform.position, forwardTarget.position, Time.deltaTime * turnSpeed / 2);
        Vector3  chestPos = chestIK.transform.position;
        chestPos.y = transform.position.y;
        chestIK.position = chestPos;*/
    }

    void Climbing()
    {
        Ray hitForward = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(hitForward.origin, hitForward.direction * 1);

        if (Physics.SphereCast(hitForward, .2f, out hit, 1.3f, 11) && Input.GetButton("PickUP") && hit.transform.tag == "climb")
            //if (Physics.Raycast(hitForward, out hit, 1.3f, 11) && Input.GetButton("PickUP") && hit.transform.tag == "climb")
            {
            climbing = true;
            useGravity = false;

             Vector3 normalRotation;

             normalRotation = new Vector3(hit.normal.x, hit.normal.y, hit.normal.z);
             transform.rotation = Quaternion.LookRotation(-normalRotation);

           // m_MyQuaternion.SetFromToRotation(transform.position, hit.normal.transform.position);

            /*Vector3 normal = hit.normal;

            Quaternion q1 = Quaternion.AngleAxis(0, normal);
            Quaternion q2 = Quaternion.FromToRotation(Vector3.up, normal);
            Quaternion quat = q1 * q2;

            transform.position = hit.point + normal * 0.5f;
            transform.rotation = quat;*/
        }
        else { climbing = false; useGravity = true; }

        if (climbing)
        {
            float deadzone = 0.5f;
            Vector3 climbStickInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            if (climbStickInput.magnitude < deadzone)
                climbStickInput = Vector3.zero;
            else
                climbStickInput = climbStickInput.normalized * ((climbStickInput.magnitude - deadzone) / (1 - deadzone));

            transform.localPosition = hit.point + hit.normal * 0.5f;

           transform.localPosition += transform.right * climbStickInput.x * Time.deltaTime * climbSpeed;
           transform.localPosition += transform.up * climbStickInput.y * Time.deltaTime * climbSpeed;

            if (Input.GetButtonUp("PickUP"))
                transform.localPosition = hit.point - hit.normal * 1.5f;
        }
    }
}
