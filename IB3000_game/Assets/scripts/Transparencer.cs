using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparencer : MonoBehaviour {

	public bool hide;
	public GameObject[] hideThese;

	private Renderer renderer;

	void Awake()
	{
		//color.a -= 0.1f;
		//renderer.material.color = color;
	}

	void FadeOut () 
	{
		for (int i = 0; i < hideThese.Length; i++) 
		{
			renderer = hideThese[i].GetComponent<Renderer> ();
			Color color = GetComponent<Renderer>().material.color;
			renderer.material.color = color;
			color.a -= Time.time;

			GetComponent<Renderer> ().material.SetColor("_Color", color);

		}
	}

	void FadeIn () 
	{
		for (int i = 0; i < hideThese.Length; i++) 
		{
			renderer = hideThese[i].GetComponent<Renderer> ();
			Color color = GetComponent<Renderer>().material.color;
			renderer.material.color = color;
			color.a += 0.05f;

			GetComponent<Renderer> ().material.SetColor("_Color", color);

		}
	}

	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag == "Player" && hide)
		FadeOut ();

		if (col.gameObject.tag == "Player" && !hide)
			FadeIn ();
	}
}
