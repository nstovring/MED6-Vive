using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour {
	public List<AudioClip>ambientSounds;
	public List<AudioClip>narrationResponse;
	public List<AudioClip>narrationQuestion;
	public List<AudioClip>ipadSounds;
	public AudioSource audio; 
	public bool playOnAwake; 
	public float volume; 

	void Start () {
		//audio = Camera.main.GetComponent<AudioSource>();
		audio = GetComponent<AudioSource>();
			
		//audio = GetComponent<AudioSource> ();

		StartCoroutine (playAmbient());
	}

	public void playAudio(AudioClip clip) {
		audio.clip = clip;
		audio.Play();
	}

	public void playCharacterResponse(){
		Debug.Log ("Playing character response");
		playAudio(narrationResponse [0]);
	}

	void Update () {
		//narration question question
		if (Input.GetKeyDown (KeyCode.A)) {
			Debug.Log ("A Pressed");
			audio.clip = narrationQuestion [0];
			//Debug.Log ("Audio clip to be inserted: " + narrationQuestion [0].name);
			//Debug.Log ("Audio clip: " + audio.clip);
			audio.Play();
		}
		else if (Input.GetKeyDown (KeyCode.B)) {
			Debug.Log ("B Pressed");
			audio.clip = narrationResponse [0];
			audio.Play();
		}

		else if (Input.GetKeyDown (KeyCode.C)) {
			Debug.Log ("C Pressed");
			audio.clip = narrationResponse [1];
			audio.Play();	
		}

		//narration Response question
		else if (Input.GetKeyDown (KeyCode.D)) {
			Debug.Log ("D Pressed");
			audio.clip = narrationResponse [0];
			audio.Play();
		}

		else if (Input.GetKeyDown (KeyCode.E)) {
			Debug.Log ("E Pressed");
			audio.clip = narrationResponse [1];
			audio.Play();
		}
		//audio.clip = ambientSounds [0]; 
		audio.playOnAwake = true;
	}

	//Background sound in another thread
	IEnumerator playAmbient() {
		//Debug.Log ("Playing ambient background sounds");
		AudioSource cameraAudio = Camera.main.GetComponent<AudioSource>();
		cameraAudio.loop = true;
		cameraAudio.clip = ambientSounds [0]; 
		cameraAudio.Play ();
		cameraAudio.volume = 0.25f; 
		yield return new WaitForSeconds(0);
		//return;
	}
}

