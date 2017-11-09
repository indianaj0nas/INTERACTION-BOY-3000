using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public GameObject liftThis;
    public float lightThrowForceUp = 200.0f;
	public float heavyThrowForceUp = 200.0f;
	public float lightThrowForce = 200.0f;
	public float heavyThrowForce = 7000.0f;

	private float currentThrowForce;
	private float currentThrowForceUp;
	private float distance;
	private bool canThrow;
	private bool canPlace;
    public bool closeEnough;
    private GameObject BOY;
    private Rigidbody rb;
	private float lastTime;
	private float throwAfterJumpTime;
	public int pressedShift;
	private bool holdingSomething;

    void Awake()
    {
		lastTime = Time.time;
		throwAfterJumpTime = Time.time;
        closeEnough = false;
		canThrow = false;
		canPlace = false;
		pressedShift = 0;
		holdingSomething = false;
    }

	void OnTriggerStay (Collider theCollision) {
		if (theCollision.gameObject.tag == "canHold" && !Input.GetButton ("PickUP") && pressedShift < 1) 
		{
			closeEnough = true;
			liftThis = theCollision.gameObject;
			rb = liftThis.GetComponent<Rigidbody> ();
		}
    }

    void LateUpdate ()
    {
        DelayAndLift();
    }

    private void DelayAndLift()
    {
        if (liftThis != null)
        {
            if (Input.GetButtonDown("Jump"))
            {
                throwAfterJumpTime = Time.time;
            }
            if (closeEnough == true)
            {
                if (Input.GetButtonDown("PickUP") && liftThis.layer == 9 && (Time.time - lastTime > 0.5f) && (Time.time - throwAfterJumpTime > 0.35f))
                {
                    pickUpObject();
                }
                if (Input.GetButtonDown("PickUP") && liftThis.layer == 8 && (Time.time - lastTime > 0.5f) && (Time.time - throwAfterJumpTime > 0.35f))
                {
                    pickUpObject();
                }
            }

            if (pressedShift == 1)
            {
                rotateIt();
                //if (liftThis == null)
                Physics.IgnoreCollision(liftThis.GetComponent<Collider>(), BOY.GetComponent<Collider>(), true);
            }

            if (liftThis != null && pressedShift == 2)
            {
                throwNow();
            }
            if (liftThis == null)
                pressedShift = 0;

            /*if (pressedShift != 1)
				liftThis = null;*/

            if (!closeEnough && liftThis != null)
            {
                //rb.useGravity = false;
                rb = null;
                liftThis.transform.parent = null;
                liftThis = null;
            }
        }
    }

    public void pickUpObject()
	{
        pressedShift = pressedShift + 1;
		lastTime = Time.time;

		rb.useGravity = false;

		rb.velocity = Vector3.zero;

		rb.isKinematic = true;
		liftThis.transform.parent = gameObject.transform;

		if (liftThis.layer == 8) 
		{
			liftThis.transform.position = gameObject.transform.position + new Vector3 (0f, 1.5f, 0f);
		}

		if (liftThis.layer == 9) 
		{
			liftThis.transform.position = gameObject.transform.position + new Vector3 (0f, 2f, 0f);
		}
    }

	public void rotateIt ()
	{
		float horizontal = Input.GetAxisRaw ("Horizontal2");
		float vertical = Input.GetAxisRaw ("Vertical2");

		liftThis.transform.Rotate (new Vector3(vertical * 2, -horizontal * 2, 0f), Space.World);
	}
	

	public void throwNow()
	{
		if (liftThis.layer == 9) 
		{
			currentThrowForce = heavyThrowForce;
			currentThrowForceUp = heavyThrowForceUp;
		}
		if (liftThis.layer == 8) 
		{
			currentThrowForce = lightThrowForce;
			currentThrowForceUp = lightThrowForceUp;
		}
		/* The throw mechanic is here*/
		if (liftThis.layer == 8 || liftThis.layer == 9) {

			if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
				Vector3 throwDirection = Vector3.zero;

				rb.useGravity = true;
				rb.isKinematic = false;

			if (Input.GetKey (KeyCode.A) || Input.GetAxis("Horizontal") == -1)
					throwDirection += new Vector3 (-1, 0, 0);
			if (Input.GetKey (KeyCode.D) || Input.GetAxis("Horizontal") == +1)
					throwDirection += new Vector3 (1, 0, 0);
			if (Input.GetKey (KeyCode.S) || Input.GetAxis("Vertical") == -1)
					throwDirection += new Vector3 (0, 0, -1);
			if (Input.GetKey (KeyCode.W) || Input.GetAxis("Vertical") == 1)
					throwDirection += new Vector3 (0, 0, 1);

				float horizontal = Input.GetAxis ("Horizontal");
				float vertical = Input.GetAxis ("Vertical");


				if (throwDirection.magnitude > 1)
					throwDirection = throwDirection *= 0.5f;

				Vector3 throwPower = throwDirection * currentThrowForce + new Vector3 (horizontal, currentThrowForceUp, vertical);

				rb.AddForce (throwPower);
				transform.localScale = new Vector3 (0f, 0f, 0f);
				pressedShift = 0;
				Invoke ("ReGrow", 0.005f);
			} else {
				putDown ();
				pressedShift = 0;
			}
		}
	}

    public void putDown() 
{
	//	TooFarAway ();

		Invoke ("BecomePhysical", 0.1f);
		if (liftThis != null) 
		{
			if (liftThis.layer == 8 || liftThis.layer == 9) 
			{

                liftThis.transform.position = transform.position + transform.forward * 3f;

                liftThis.transform.parent = null;
				//liftThis = null;
				pressedShift = 0;
				//Invoke ("BecomePhysical", 3f);

				//Vector3 throwPower = gameObject.transform.position + new Vector3 (0, -1, 0);
				Invoke ("ReGrow", 1f);
			}
		}
}

	public void ReGrow() 
	{
		transform.localScale = new Vector3(1f,1f,1f);
		pressedShift = 0;
	}

	public void BecomePhysical()
	{
		rb.useGravity = true;
		rb.isKinematic = false;
		liftThis = null;
	}

	public void TooFarAway()
	{
		distance = Vector3.Distance (gameObject.transform.position, liftThis.transform.position);

		if (distance > 4) 
		{
			rb.useGravity = true;
			rb.isKinematic = false;
			liftThis.transform.parent = null;
			liftThis = null;
		}
	}

	public void TooFarAway2()
	{
			//rb.useGravity = true;
			//rb.isKinematic = false;
			liftThis.transform.parent = null;
			liftThis = null;
	}

    void OnTriggerExit(Collider liftThis)
    {

		if (liftThis == null) 
		{
			pressedShift = 0;
		}

		if (liftThis != null && pressedShift == 0) {
			closeEnough = false;
			//liftThis = null;
		}
    }
}