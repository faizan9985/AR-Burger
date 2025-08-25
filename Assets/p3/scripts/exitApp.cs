/*
 * This script controls the user driven exiting from the app
 * it is placed in a scripts container gameobject
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitApp : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		//listen for a keypress
		if (Input.GetKey (KeyCode.F12) || Input.GetKey(KeyCode.JoystickButton3) || Input.GetKey(KeyCode.JoystickButton19))
			doExit (); //if a keypress action is found then the doExit method is called
	}

	public void doExit()
	{
		//test to see if the application is running in the Editor
		#if UNITY_EDITOR
			// use this command to stop the app if running in the editor
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			// use this command to stop the app if NOT running in the editor
			Application.Quit(); 
		#endif
	}
    public void OnTriggerEnter(Collider other)
    {
		doExit();

	}
}
