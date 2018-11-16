using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigationManager : MonoBehaviour {

    public string previousScene;
	
	void Update ()
    {
        if (Input.GetButtonDown("Cancel") && previousScene != string.Empty)
        {
            SceneTransition sceneTransition = SceneTransition.instance;
            if (sceneTransition) sceneTransition.Call(previousScene);
            else                 SceneTransition.LoadScene(previousScene);
        }	
	}
}
