using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour {

	public Transform PauseMenu;

	//public string thisScene;
	

	void Update () {
		if (Input.GetButtonDown ("Pause")) 
		{
			Pause ();
			//Scene thisScene = SceneManager.GetActiveScene();
		} 
	}

	public void Pause()
	{
		if (PauseMenu.gameObject.activeInHierarchy == false) {
			PauseMenu.gameObject.SetActive (true);
			Time.timeScale = 0f;
		}
		else if (PauseMenu.gameObject.activeInHierarchy == true) {
			Time.timeScale = 1;
			PauseMenu.gameObject.SetActive(false);
		}
	}

	public void Quit()
	{
		Application.Quit ();
	}

	public void Restart()
	{
		Application.LoadLevel (Application.loadedLevel);
	//	SceneManager.LoadScene("thisScene");
		Time.timeScale = 1;
	}

}
