using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidingDoor : MonoBehaviour {

    public GameObject theDoor;
    public float speed = 1f;

    Vector3 closedPosition = new Vector3(0f, 0f, 0f);
    Vector3 openPosition;

    bool gotTriggered=false;


    void Start () {
        openPosition = new Vector3(0f, 5f, 0f) + theDoor.transform.position;
        closedPosition = theDoor.transform.position;
    }
	
	void OnTriggerEnter (Collider other) {
        gotTriggered = true;
        
	}

    void OnTriggerExit(Collider other)
    {
        gotTriggered = false;

    }

    void Update()
    {
        
        if (gotTriggered)
        {
            float step = speed * Time.deltaTime;
            theDoor.transform.position = Vector3.MoveTowards(theDoor.transform.position, openPosition, step);
        }
        else
        {
            float step = speed * Time.deltaTime;
            theDoor.transform.position = Vector3.MoveTowards(theDoor.transform.position, closedPosition, step);
        }
       
    }
}
