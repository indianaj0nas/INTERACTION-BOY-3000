using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {
	 
	public static SceneManagerScript Instance { set; get;}


	void Awake () 
	{
		Instance = this;
	//	Load ("Scene1");
	//	Load ("Scene2");
	}

	public void Load (string sceneName)
	{
		if (!SceneManager.GetSceneByName (sceneName).isLoaded)
			SceneManager.LoadScene (sceneName, LoadSceneMode.Additive);
	}

	public void Unload (string sceneName)
	{
		if (SceneManager.GetSceneByName (sceneName).isLoaded)
			SceneManager.UnloadScene (sceneName);
	}
}