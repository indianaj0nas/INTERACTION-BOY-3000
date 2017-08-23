using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour {

	public int targetFrameRate = 60;

	// Use this for initialization
	void Awake () {
		Application.targetFrameRate = targetFrameRate;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
