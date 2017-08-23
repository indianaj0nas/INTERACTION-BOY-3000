using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class growingObject : MonoBehaviour {

	public float xSize = 10f;
	public float ySize = 10f;
	public float zSize = 10f;

	private Vector3 originalScale;
	private Vector3 scaleUp;
	private GameObject player;

	private Rigidbody rb;

	void Awake() 
	{
		originalScale = transform.localScale;
		scaleUp = new Vector3 (xSize, ySize, zSize);
		rb = GetComponent<Rigidbody>();
		player = GameObject.Find ("BOY");
	}

	void Update()
	{
		if (rb.useGravity == false || rb.velocity.y > 0.3f && rb.velocity.z > 0.3f && rb.velocity.x > 0.3f )
		{
			transform.localScale = originalScale;
		}
	}

	void OnCollisionStay()
	{
		//if (theCollision.gameObject.tag == "Player") 
		if (rb.useGravity == true && transform.localScale.x < xSize && rb.velocity.y < 0.1f && rb.velocity.z < 0.1f && rb.velocity.x < 0.1f) 
		{
			transform.localScale += scaleUp * Time.deltaTime;
		}
	}
}