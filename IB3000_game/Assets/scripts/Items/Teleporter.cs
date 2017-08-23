using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

	public GameObject teleportThis;
	public int pressedShift;

	void Update () {

		GameObject thePickUpZone = GameObject.Find ("pickUpZone");
		PickUp pickUp = thePickUpZone.GetComponent<PickUp> ();
		if (Input.GetKeyDown (KeyCode.E) && pickUp.pressedShift != 1) 
		{
			teleportThis.transform.position = transform.position + new Vector3(0f, 1f, 0f);
		}
	}
}
