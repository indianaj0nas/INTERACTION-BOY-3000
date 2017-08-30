using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

	public GameObject poof;
	public Transform projectile;
	public GameObject shootPoint;
	public GameObject aimer;
	public float shotStrength;

	public GameObject liftThis;

	private float lastTime;
	private Rigidbody rb;
	private bool canPoof;
	private bool shot;
	private GameObject Player;

	void Start ()
	{
		lastTime = Time.time;
		Player = GameObject.Find ("BOY");
		//poof.GetComponent<ParticleSystem> ().enableEmission = false;
	}

	void Update () {

		if (Input.GetButtonUp ("Action")  && (Time.time - lastTime > 0.5f)) 
		{
			Shoot ();
			shot = true;
		}

		if (shot) 
		{
			Rekyl ();
		}

		if (Input.GetButtonDown ("Action")) 
		{
			transform.localScale = new Vector3 (1.1f, 1.1f, .9f);
			poof.SetActive (false);
		}
	}

	void OnCollisionStay(Collision col)
	{
		if (col.gameObject.tag == "Player") {
			aimer.gameObject.SetActive (true);
		} else if (col.gameObject.tag == "Player"){
			aimer.gameObject.SetActive (false);
		}
	}

	void Rekyl()
	{
		GameObject thePickUpZone = GameObject.Find ("pickUpZone");
		PickUp pickUp = thePickUpZone.GetComponent<PickUp> ();
		gameObject.GetComponent<Rigidbody> ().AddRelativeForce (Vector3.back * 500);
		if (pickUp.liftThis == gameObject) 
		{
			//Player.GetComponent<Rigidbody> ().AddRelativeForce (Vector3.back * 2000);
			Player.GetComponent<Rigidbody> ().AddRelativeForce (Player.transform.position -= transform.forward * Time.deltaTime * 50);
		}
		
		stopRekyl ();
		Invoke ("stopRekyl", 3f);
	}

	void Shoot() 
	{
		lastTime = Time.time;
		Instantiate (projectile, shootPoint.transform.position, transform.rotation);
		rb = projectile.GetComponent<Rigidbody>();
		transform.localScale = new Vector3 (1f, 1f, 1f);
		poof.SetActive (true);
	}

	void stopRekyl()
	{
		shot = false;
	}
}
