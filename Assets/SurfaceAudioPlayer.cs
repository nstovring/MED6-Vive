using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SurfaceAudioPlayer : MonoBehaviour {

    public List<AudioSource> aSource;

    public List<AudioMixerSnapshot> snapshots;

    float changeThreshold = 0.5f;

    int currentSound = 0;
	// Use this for initialization
	void Start () {
        changeThreshold = Random.Range(-1, 1);
	}

    public string GetAccuracy()
    {
        return "Threshold :" + changeThreshold + " User Selected Pos" + transform.position.z;
    }
	
	// Update is called once per frame
	void Update () {
		if(transform.localPosition.z > changeThreshold)
        {
            snapshots[1].TransitionTo(0.1f);
            //aSource[1].volume = Mathf.Lerp(aSource[currentSound].volume, 1, fingerRb.velocity.sqrMagnitude / 10);
            //aSource[0].volume = Mathf.Lerp(aSource[currentSound].volume, 0, fingerRb.velocity.sqrMagnitude / 10);
        }
        else
        {
            snapshots[0].TransitionTo(0.1f);
        }
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
            transform.position = new Vector3(transform.position.x, transform.position.y, fingerRb.position.z); 
            aSource[0].volume = Mathf.Lerp(aSource[0].volume,1,fingerRb.velocity.sqrMagnitude / 10);
            aSource[1].volume = Mathf.Lerp(aSource[1].volume, 1, fingerRb.velocity.sqrMagnitude / 10);
        }
    }
    void OnCollisionExit(Collision other)
    {
        aSource[0].volume = 0;
        aSource[1].volume = 0;
        fingerRb = null;
    }


}
