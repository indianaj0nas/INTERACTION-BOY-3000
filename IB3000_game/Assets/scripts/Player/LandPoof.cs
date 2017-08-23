using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandPoof : MonoBehaviour {

	public Transform poof;

	void Start () {
		//Physics.IgnoreLayerCollision (11, 11, true);
		poof.GetComponent<ParticleSystem> ().enableEmission = false;
	}
		
	void OnTriggerEnter()
	{
		//if (rb.velocity.y > 0.1f) {
		//	poof.GetComponent<ParticleSystem> ().enableEmission = true;
			StartCoroutine (stopPoof ());
		//}
	}

	IEnumerator stopPoof()
	{
		yield return new WaitForSeconds (0.1f);
		//poof.GetComponent<ParticleSystem> ().emission.enabled;
	}

}
