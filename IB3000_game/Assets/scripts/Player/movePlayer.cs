using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayer : MonoBehaviour {

    public Transform poof;
    public float walkSpeed;
    public float runSpeed;
    public float jumpHeight = 200.0f;
    public int pressedShift;
    public GameObject liftThis;
    public float climbSpeed;
    public float turnSpeed;

    private Rigidbody rb;
    private bool canPoof;
    private bool currentlyClimbing;
    private bool canJump;
    Animator anim;
    private float speed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        poof.GetComponent<ParticleSystem>().enableEmission = false;
        Physics.IgnoreLayerCollision(11, 11, true);
    }

    void FixedUpdate()
    {
        GameObject thePickUpZone = GameObject.Find("pickUpZone");
        PickUp pickUp = thePickUpZone.GetComponent<PickUp>();

        Walk();
        Run();

        Lifting(pickUp);

        Jump();
    }

    private void Lifting(PickUp pickUp)
    {
        if (pickUp.pressedShift == 1)
        {
            anim.SetBool("holding", true);
            Physics.IgnoreCollision(GetComponent<Collider>(), pickUp.liftThis.GetComponent<Collider>(), true);
        }
        if (pickUp.pressedShift == 0)
        {
            anim.SetBool("holding", false);
            if (pickUp.liftThis != null)
                Invoke("turnOnPhysics", .2f);
        }
    }

    private void Jump()
    {
        RaycastHit hitDown;
        float theDistanceDown;
        Vector3 down = transform.TransformDirection(Vector3.down) * 1.3f;
        Debug.DrawRay(transform.position, down, Color.red);
        if (Physics.Raycast(transform.position, (down), out hitDown))
        {
            theDistanceDown = hitDown.distance;
            if (rb.velocity.y < 1f && currentlyClimbing == false)
            {
                anim.SetBool("Falling", true);
            }
            else
            {
                anim.SetBool("Falling", false);
            }

            if (rb.velocity.y > 1f && theDistanceDown > 1.3f && currentlyClimbing == false)
            {
                anim.SetBool("Jumping", true);
            }
            else
            {
                anim.SetBool("Jumping", false);
            }
            if (theDistanceDown < 1.3f && currentlyClimbing == false)
            {
                anim.SetBool("Falling", false);
            }

            if (theDistanceDown >= 6f)
            {
                canPoof = true;
            }

            if (canPoof == true && theDistanceDown < 1.3f)
            {
                poof.GetComponent<ParticleSystem>().enableEmission = true;
                StartCoroutine(stopPoof());
            }

            if (Input.GetButtonDown("Jump") && theDistanceDown < 1.2f && hitDown.transform.gameObject.tag != "climb")
            {
                Vector3 jump = new Vector3(0.0f, jumpHeight, 0.0f);
                rb.AddForce(jump);
            }
        }
    }

    private void Walk()
    {
        //rb.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, rb.velocity.y, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);

        //
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        if (movement.sqrMagnitude > 0.1f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), turnSpeed);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f && currentlyClimbing == false) { anim.SetBool("Moving", true); }
        else { anim.SetBool("Moving", false); }
    }

    private void Run()
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
    }

    void OnTriggerExit(Collider col)
    {
        StopClimbing(col);
    }

    private void StopClimbing(Collider col)
    {
        if (col.gameObject.tag == "climb")
        {
            currentlyClimbing = false;
            rb.useGravity = true;
            anim.SetBool("climbing", false);
        }
        GameObject thePickUpZone = GameObject.Find("pickUpZone");
        PickUp pickUp = thePickUpZone.GetComponent<PickUp>();
        if (col.gameObject == pickUp.liftThis)
            Physics.IgnoreCollision(GetComponent<Collider>(), pickUp.liftThis.GetComponent<Collider>(), false);
    }

    void OnTriggerStay(Collider theCollision)
    {
        StartClimbing(theCollision);
    }

    private void StartClimbing(Collider theCollision)
    {
        if (theCollision.gameObject.tag == "climb")
        {
            //if (currentlyClimbing && Input.GetButton("PickUP"))
            //	rb.velocity = new Vector3 (0f, 0f, 0f);

            if (Input.GetButton("PickUP"))
            {
                currentlyClimbing = true;
                anim.SetBool("Jumping", false);
                anim.enabled = true;
                //liftThis.transform.parent = null;

                rb.useGravity = false;
                rb.AddForce(0, climbSpeed * Time.deltaTime, 0, ForceMode.VelocityChange);
            }

            if (currentlyClimbing)
            {
                rb.MovePosition(transform.position);
                anim.SetBool("climbing", true);
                if (Input.GetButtonUp("PickUP"))
                {
                    //anim.enabled = false;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                if (rb.velocity.magnitude < 0.09f)
                    anim.enabled = false;

                if (rb.velocity.magnitude >= 0.1f)
                {
                    anim.enabled = true;
                }
            }
        }

        GameObject thePickUpZone = GameObject.Find("pickUpZone");
        PickUp pickUp = thePickUpZone.GetComponent<PickUp>();
        if (theCollision.gameObject == pickUp.liftThis)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), pickUp.liftThis.GetComponent<Collider>(), true);

        }
    }

    public void turnOnPhysics() 
	{
		GameObject thePickUpZone = GameObject.Find ("pickUpZone");
		PickUp pickUp = thePickUpZone.GetComponent<PickUp> ();
		if (pickUp.liftThis != null)
		Physics.IgnoreCollision (GetComponent<Collider>(), pickUp.liftThis.GetComponent<Collider>(), false);
	}

	IEnumerator stopPoof()
	{
		yield return new WaitForSeconds (0.1f);
		poof.GetComponent<ParticleSystem> ().enableEmission = false;
		canPoof = false;
	}
}