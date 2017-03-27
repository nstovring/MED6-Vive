using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {
	public AudioClip[] ambientSounds = new AudioClip[1];
	public AudioClip[] narrationResponse = new AudioClip[2];
	public AudioClip[] narrationQuestion = new AudioClip[1];
	public AudioClip [] ipadSounds = new AudioClip[2];
	public GameObject narrationSounds;
	public AudioSource ambientJingle;
	public bool playOnAwake = false; 

	void Update () {
		//narration Sounds question
		if (Input.GetKeyDown (KeyCode.C)) {
			AudioSource audio = narrationSounds.GetComponent<AudioSource> ();
			audio.clip = narrationQuestion [1];
			audio.Play ();
			Debug.Log ("C Pressed");
		}

		//narration sounds response
		if (Input.GetKeyDown (KeyCode.A)) {
			Debug.Log ("a Pressed");
			AudioSource audio = narrationSounds.GetComponent<AudioSource> ();
			audio.clip = narrationResponse [0];
			audio.Play ();
		}
		if (Input.GetKeyDown (KeyCode.B)) {
			AudioSource audio = narrationSounds.GetComponent<AudioSource> ();
			audio.clip = narrationResponse [1];
			audio.Play ();
			Debug.Log ("B Pressed");
		}

	}

	void ambient(){
		AudioSource audio = ambientJingle.GetComponent<AudioSource> ();
		audio.clip = ambientSounds [0];
		audio.Play ();
		playOnAwake = true;
	}
}

