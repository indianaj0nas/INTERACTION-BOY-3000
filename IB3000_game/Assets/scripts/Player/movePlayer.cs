using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayer : MonoBehaviour {
	
	public Transform poof;
    public float moveSpeed;
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

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
		poof.GetComponent<ParticleSystem> ().enableEmission = false;
		Physics.IgnoreLayerCollision (11, 11, true);
    }

    /*void FixedUpdate()
    {
		

		if(currentlyClimbing)
		Debug.Log ("true");
		if(!currentlyClimbing)
			Debug.Log ("false");
    } */

    void FixedUpdate()
    {
		GameObject thePickUpZone = GameObject.Find ("pickUpZone");
		PickUp pickUp = thePickUpZone.GetComponent<PickUp> ();

		//rb.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, rb.velocity.y, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);

		//
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
		//transform.rotation = Quaternion.LookRotation(movement);
		if (movement.sqrMagnitude > 0.1f)
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), turnSpeed);

		transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
		//rb.AddForce(movement * speed, Space.World);

		//

		if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f && currentlyClimbing == false) { anim.SetBool("Moving", true); }
		else { anim.SetBool("Moving", false); }

		if (pickUp.pressedShift == 1) 
		{ 
			anim.SetBool ("holding", true);
			Physics.IgnoreCollision (GetComponent<Collider>(), pickUp.liftThis.GetComponent<Collider>(), true);
		}
		if (pickUp.pressedShift == 0) 
		{
			anim.SetBool ("holding", false);
			if (pickUp.liftThis != null)
				Invoke("turnOnPhysics", .2f);
		}
	
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
				poof.GetComponent<ParticleSystem> ().enableEmission = true;
				StartCoroutine (stopPoof ());
			}

			if (Input.GetButtonDown("Jump") && theDistanceDown < 1.2f && hitDown.transform.gameObject.tag != "climb")
            {
                Vector3 jump = new Vector3(0.0f, jumpHeight, 0.0f);
				rb.AddForce(jump);
            }
        }
    }

	void OnTriggerExit(Collider col) 
	{
		if (col.gameObject.tag == "climb") 
		{
			currentlyClimbing = false;
			rb.useGravity = true;
			anim.SetBool ("climbing", false);
		}
		GameObject thePickUpZone = GameObject.Find ("pickUpZone");
		PickUp pickUp = thePickUpZone.GetComponent<PickUp> ();
		if (col.gameObject == pickUp.liftThis)
		Physics.IgnoreCollision (GetComponent<Collider>(), pickUp.liftThis.GetComponent<Collider>(), false);
	}

	void OnTriggerStay(Collider theCollision) 
	{
		if (theCollision.gameObject.tag == "climb") {
			//if (currentlyClimbing && Input.GetButton("PickUP"))
			//	rb.velocity = new Vector3 (0f, 0f, 0f);
		
			if (Input.GetButton("PickUP")) 
			{
				currentlyClimbing = true;
				anim.SetBool ("Jumping", false);
				anim.enabled = true;
				//liftThis.transform.parent = null;

				rb.useGravity = false;
				rb.AddForce(0, climbSpeed * Time.deltaTime, 0, ForceMode.VelocityChange);
			}

			if (currentlyClimbing) 
			{
				rb.MovePosition(transform.position);
				anim.SetBool ("climbing", true);
				if (Input.GetButtonUp ("PickUP")) 
				{
					//anim.enabled = false;
					rb.velocity = Vector3.zero;
					rb.angularVelocity = Vector3.zero;
				}

				if (rb.velocity.magnitude < 0.09f ) 
					anim.enabled = false;
				
				if (rb.velocity.magnitude >= 0.1f ) 
				{
					anim.enabled = true;
				}
			}



				//if (Input.GetButton("PickUP"))
				//rb.velocity = new Vector3(0f, -6f, 0f);
		}
		//if (theCollision.gameObject.tag != "climb") 
		//{
		//	anim.SetBool ("climbing", false);
		//	canJump = true;
		//}

		GameObject thePickUpZone = GameObject.Find ("pickUpZone");
		PickUp pickUp = thePickUpZone.GetComponent<PickUp> ();
		if (theCollision.gameObject == pickUp.liftThis) 
		{
			Physics.IgnoreCollision (GetComponent<Collider>(), pickUp.liftThis.GetComponent<Collider>(), true);

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