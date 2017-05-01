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
    Rigidbody fingerRb;
    void OnCollisionStay(Collision other)
    {
        Debug.Log("Collision");
        if(fingerRb == null)
        {
            fingerRb = other.transform.GetComponent<Rigidbody>();
        }

        if(fingerRb != null)
        {
            aSource.volume = Mathf.Lerp(aSource.volume,1,fingerRb.velocity.sqrMagnitude/10);
        }
    }
    void OnCollisionExit(Collision other)
    {
        aSource.volume = 0;
        fingerRb = null;
    }


}
