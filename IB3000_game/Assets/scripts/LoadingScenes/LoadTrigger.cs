using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTrigger : MonoBehaviour {

	public string loadName;
	public string unloadName;

	void OnTriggerEnter (Collider col) 
	{
		if (loadName != "")
			SceneManagerScript.Instance.Load (loadName);

		if (unloadName != "")
			StartCoroutine ("UnloadScene");
	}

	IEnumerator UnloadScene()
	{
		yield return new WaitForSeconds (.10f);
		SceneManagerScript.Instance.Unload (unloadName);
	}
}