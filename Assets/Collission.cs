using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collission : MonoBehaviour {

	private AudioSource audio;
	public List<AudioClip> soundClips = new List<AudioClip>();
	//private bool loop = false;

	// Use this for initialization
	void Start () {
		audio = Camera.main.GetComponent<AudioSource> ();
		audio.volume = 1;
	}
	
	// Update is called once per frame
	void Update () {
		/**if (loop) {

		}*/
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Entering object " + other.name);
		if (other.tag == "surface") {
			audio.clip = soundClips[0];
			audio.loop = true;
			audio.Play ();
		}
	}

	void OnTriggerExit(Collider other) {
		Debug.Log ("Exiting object " + other.name);
		audio.Stop ();
	}

	void OnTriggerStay(Collider other) {
		Debug.Log ("Moving within object " + other.name);

		/**
		if (other.tag == "surface") {
			Debug.Log ("Touching surface. Vibrating");
			loop = true;
			audio.clip = soundClips[0];
		}*/
	}
}
