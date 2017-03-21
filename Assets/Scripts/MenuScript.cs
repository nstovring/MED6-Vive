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
