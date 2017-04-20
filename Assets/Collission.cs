using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collission : MonoBehaviour {

	private AudioSource audio;
	public List<AudioClip> soundClips = new List<AudioClip>();

    private Rigidbody rb;
	//private bool loop = false;

	// Use this for initialization
	void Start () {
		audio = Camera.main.GetComponent<AudioSource> ();
        rb = GetComponent<Rigidbody>();
		audio.volume = 1;
	}
	
	// Update is called once per frame
	void Update () {
		/**if (loop) {

		}*/
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Entering object " + other.name +"Velocity " + rb.velocity.magnitude);
		if (other.tag == "surface") {
			audio.clip = soundClips[0];
			audio.loop = true;
			audio.Play ();
        }
        else
        {
            audio.Stop();
        }
	}

	void OnTriggerExit(Collider other) {
		Debug.Log ("Exiting object " + other.name);
		audio.Stop ();
	}

	void OnTriggerStay(Collider other) {
		Debug.Log ("Moving within object " + other.name);


        //if (other.tag == "surface" && !audio.isPlaying && rb.velocity.magnitude > 0.25f)
        //{
        //    audio.clip = soundClips[0];
        //    audio.loop = true;
        //    audio.Play();
        //}
        //else
        //{
        //    audio.Stop();
        //}
        /**
		if (other.tag == "surface") {
			Debug.Log ("Touching surface. Vibrating");
			loop = true;
			audio.clip = soundClips[0];
		}*/
    }
}
