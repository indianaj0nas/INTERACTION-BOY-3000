using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showText : MonoBehaviour {

    public GameObject theText;
    public GameObject Xicon;

    private bool isVisible;

    void Start()
    {
        isVisible = false;
    }

    void OnTriggerStay(Collider other)
    {

		if (other.gameObject.tag == "Player") 
		{
			Xicon.SetActive(true);
			if (Input.GetKeyDown(KeyCode.X) && isVisible == false)
			{
				isVisible = true;
				theText.SetActive(true);
				Xicon.SetActive(false);
			}
		}
       
    }

    void OnTriggerExit(Collider other)
    {
        isVisible = false;
        Xicon.SetActive(false);
        theText.SetActive(false);
    }
}
