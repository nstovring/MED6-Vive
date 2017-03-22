using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public GameObject objectClicked;

	private VRCameraFade myFade;

	// Use this for initialization
	void Start () {
		new WaitForSeconds(2);
		myFade = Camera.main.GetComponent<VRCameraFade>();
		myFade.FadeIn(2, false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			Debug.Log ("1 pressed");
			myFade.FadeOut(2, false);
			Debug.Log ("Fading");
			StartCoroutine(waitAndLoad (2, "ClassRoom"));
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2)){
			Debug.Log("Scene 2 pressed");
			myFade.FadeOut(2, false);
			StartCoroutine(waitAndLoad (2, "Scene2"));
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3)){
			Debug.Log("Scene 3 pressed");
			myFade.FadeOut(2, false);
			StartCoroutine(waitAndLoad (2, "Scene3"));
		}

	}

	void OnGUI() {
		//Debug.Log ("Button down. Alpha 1 to string: " + KeyCode.Alpha1.ToString());
		if (Event.current.Equals (Event.KeyboardEvent (KeyCode.Alpha1.ToString()))) {
			//Debug.Log ("Button 1 down");
		}
	}

	//Click a menu item
	void OnMouseDown() {
		//Debug.Log("Scene " + objectClicked.name + " pressed");

		if (objectClicked.name == "PlaneA") {
			Debug.Log("Scene PlaneA pressed");
			myFade.FadeOut(2, false);
			Debug.Log ("Fading");
			StartCoroutine(waitAndLoad (2, "ClassRoom"));

			//waitAndLoad (2, "ClassRoom");
			//Debug.Log ("Done loading");

			//SceneManager.LoadScene("ClassRoom");
		}
		else if (objectClicked.name == "PlaneB") {
			Debug.Log("Scene PlaneB pressed");
			myFade.FadeOut(2, false);
		}
		else if (objectClicked.name == "PlaneC") {
			Debug.Log("Scene PlaneC pressed");
			myFade.FadeOut(2, false);
		}
	}

	IEnumerator waitAndLoad(int seconds, string scene) {
		Debug.Log ("Waiting before load");
		yield return new WaitForSeconds(seconds);
		Debug.Log ("Loading");
		SceneManager.LoadScene(scene);
	}
}
