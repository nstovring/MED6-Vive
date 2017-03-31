using UnityEngine;
using System.Collections.Generic;

public class Sound : MonoBehaviour {
	public List<AudioClip>ambientSounds;
	public List<AudioClip>narrationResponse;
	public List<AudioClip>narrationQuestion;
	public List<AudioClip>ipadSounds;
	private AudioSource audio; 
	public bool playOnAwake; 

	void start () {
		audio = GetComponent<AudioSource> ();
	}

	public void update () {
		//narration question question
		if (Input.GetKeyDown (KeyCode.A)) {
			Debug.Log ("C Pressed");
			audio.clip = narrationQuestion [0];
			audio.Play();
		}
		if (Input.GetKeyDown (KeyCode.B)) {
			Debug.Log ("B Pressed");
			audio.clip = narrationResponse [0];
			audio.Play();
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			Debug.Log ("D Pressed");
			audio.clip = narrationResponse [1];
			audio.Play();
		}

		//narration Response question
		if (Input.GetKeyDown (KeyCode.B)) {
			Debug.Log ("B Pressed");
			audio.clip = narrationResponse [0];
			audio.Play();
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			Debug.Log ("D Pressed");
			audio.clip = narrationResponse [1];
			audio.Play();
		}
		audio.clip = ambientSounds [0]; 
		audio.playOnAwake = true; 
	}
}

