using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {

	public GameObject objectClicked;
	public GameObject screen;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {


	}

	//Click a button
	void OnMouseDown() {

		if (objectClicked.name == "YellowButton") {
			Debug.Log("Yellow button pressed");

			//If color is correct
			if (screen.GetComponent<Renderer> ().material.color == new Color (1, 1, 0)) {
				Debug.Log ("Correct color chosen");
				screen.GetComponent <Renderer> ().material.color = new Color (0,1,0); 
			} else {
				Debug.Log ("Wrong color chosen");
				screen.GetComponent <Renderer> ().material.color = new Color (1,0,0); 

			}
		}
		else if (objectClicked.name == "PurpleButton") {
			Debug.Log("Purple button pressed");

			if (screen.GetComponent<Renderer> ().material.color == new Color (1, 0, 1)) {
				Debug.Log ("Correct color chosen");
				screen.GetComponent <Renderer> ().material.color = new Color (0,1,0); 

			} else {
				Debug.Log ("Wrong color chosen");
				screen.GetComponent <Renderer> ().material.color = new Color (1,0,0); 
			}
		}
		else if (objectClicked.name == "BlueButton") {
			Debug.Log("Blue button pressed");

			if (screen.GetComponent<Renderer> ().material.color == new Color (0, 0, 1)) {
				Debug.Log ("Correct color chosen");
				screen.GetComponent <Renderer> ().material.color = new Color (0,1,0); 

			} else {
				Debug.Log ("Wrong color chosen");
				screen.GetComponent <Renderer> ().material.color = new Color (1,0,0); 
			}
		}
	}
}
