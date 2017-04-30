using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceAudioPlayer : MonoBehaviour {

    AudioSource aSource;
	// Use this for initialization
	void Start () {
        aSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionStay(Collider other)
    {

    }
}
