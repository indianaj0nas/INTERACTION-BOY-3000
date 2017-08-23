using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourItems : MonoBehaviour {

	public GameObject item1;
	public GameObject gItem1;
	public Transform item2;
	public Transform item3;
	public Transform item4;

	private bool created;

	void Start()
	{
		created = false;
	}

	void Update () {
		if (Input.GetAxisRaw("dPadHorizontal") == -1)
		{
			if (gItem1 == null) 
			{
				Instantiate (item1, transform.position + Vector3.up * 2, Quaternion.identity);
				gItem1 = item1;
				created = true;
			}
		}
	/*	if (Input.GetAxisRaw("dPadHorizontal") == 1)
		{
			Instantiate (item2, transform.position + Vector3.up * 2, Quaternion.identity);
		}
		if (Input.GetAxisRaw("dPadVertical") == -1)
		{
			Instantiate (item3, transform.position + Vector3.up * 2, Quaternion.identity);
		}
		if (Input.GetAxisRaw("dPadVertical") == 1)
		{
			Instantiate (item4, transform.position + Vector3.up * 2, Quaternion.identity);
		}*/
	}
}
