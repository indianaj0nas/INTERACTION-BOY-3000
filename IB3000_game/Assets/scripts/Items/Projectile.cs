using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float speed = 1f;

	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void Update () {
		if (rb.isKinematic == true)
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
		Destroy(gameObject, 2.5f);
		RaycastHit hit;
		if (rb.SweepTest (transform.forward, out hit, 0.4f)) 
		{
			if (hit.collider.gameObject.tag != "canHold")
			rb.isKinematic = false;
		}
	}

	void OnCollisionEnter () 
	{
		rb.isKinematic = false;
	}
}
