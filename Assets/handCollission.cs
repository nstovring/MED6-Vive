using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handCollission : MonoBehaviour {

	private Sound soundScript;
	public GameObject parentClassmate;
	public GameObject screen;
	public bool volume; 
	// Use this for initialization
	void Start () {
		soundScript = parentClassmate.GetComponent<Sound> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col) {
		Debug.Log ("Collision");
		if (col.gameObject.name == "YellowButton") {
			Debug.Log ("Collision with yellow button");
			yellowButtonPressed ();
		}
		else if (col.gameObject.name == "BlueButton") {
			Debug.Log ("Collision with blue button");
			blueButtonPressed ();
		}
		else if (col.gameObject.name == "PurpleButton") {
			Debug.Log ("Collision with purple button");
			purpleButtonPressed ();
		}
	}

	private void yellowButtonPressed() {
		Debug.Log ("Yellow button pressed");
		//If color is correct
		if (screen.GetComponent<Renderer> ().material.color == new Color (1, 1, 0)) {
			Debug.Log ("Correct color chosen");
			screen.GetComponent <Renderer> ().material.color = new Color (0, 1, 0);
			soundScript.playAudio(soundScript.ipadSounds[0]);
			AudioSource ipadSound = soundScript.playAudio (soundScript.ipadSounds [1]);
			ipadSound.volume = 0.35f; 

		} else {
			Debug.Log ("Wrong color chosen");
			screen.GetComponent <Renderer> ().material.color = new Color (1, 0, 0); 
			soundScript.playAudio (soundScript.ipadSounds [1]);
			AudioSource ipadSound = soundScript.playAudio (soundScript.ipadSounds [1]);
			ipadSound.volume = 0.35f; 

		}
		screen.GetComponent<ScreenColor>().playing = false;
	}

	private void purpleButtonPressed() {
		Debug.Log("Purple button pressed");
		if (screen.GetComponent<Renderer> ().material.color == new Color (1, 0, 1)) {
			Debug.Log ("Correct color chosen");
			screen.GetComponent <Renderer> ().material.color = new Color (0,1,0);
			soundScript.playAudio(soundScript.ipadSounds[0]);
			AudioSource ipadSound = soundScript.playAudio (soundScript.ipadSounds [1]);
			ipadSound.volume = 0.35f; 
		} else {
			Debug.Log ("Wrong color chosen");
			screen.GetComponent <Renderer> ().material.color = new Color (1,0,0); 
			soundScript.playAudio(soundScript.ipadSounds[1]);
			AudioSource ipadSound = soundScript.playAudio (soundScript.ipadSounds [1]);
			ipadSound.volume = 0.35f; 
		}
		screen.GetComponent<ScreenColor>().playing = false;
	}

	private void blueButtonPressed() {
		Debug.Log("Blue button pressed");
		if (screen.GetComponent<Renderer> ().material.color == new Color (0, 0, 1)) {
			Debug.Log ("Correct color chosen");
			screen.GetComponent <Renderer> ().material.color = new Color (0,1,0); 	
			soundScript.playAudio(soundScript.ipadSounds[0]);
			AudioSource ipadSound = soundScript.playAudio (soundScript.ipadSounds [1]);
			ipadSound.volume = 0.35f; 
		} else {
			Debug.Log ("Wrong color chosen");
			screen.GetComponent <Renderer> ().material.color = new Color (1,0,0);
			soundScript.playAudio(soundScript.ipadSounds[1]);
			AudioSource ipadSound = soundScript.playAudio (soundScript.ipadSounds [1]);
			ipadSound.volume = 0.35f; 
		}
		screen.GetComponent<ScreenColor>().playing = false;
	}
}
