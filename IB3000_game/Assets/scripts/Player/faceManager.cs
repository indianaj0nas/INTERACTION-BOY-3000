using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceManager : MonoBehaviour {

    public GameObject face;

    public Transform faceBone;

	void Awake ()
    {
        face.transform.parent = faceBone;
        face.transform.position = faceBone.transform.position;
    }
}
