using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SurfaceAudioPlayer : MonoBehaviour {

    public List<AudioSource> aSource;

    public List<AudioMixerSnapshot> snapshots;

    public float changeThreshold = 0.5f;

    int currentSound = 0;

    Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        changeThreshold = Random.Range(-1.0f, 1.0f);
	}

    public string GetAccuracy()
    {
        return  changeThreshold + ", " + transform.localPosition.z;
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

        if (transform.localPosition.z < -1)
        {
            transform.localPosition  = new Vector3(transform.localPosition.x, transform.localPosition.y, -1);
        }
        if (transform.localPosition.z > 1)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1);
        }
    }
    Rigidbody fingerRb;
    void OnCollisionStay(Collision other)
    {
        if (other.transform.tag == "IndexTop")
        {
            if (fingerRb == null)
            {
                fingerRb = other.transform.GetComponent<Rigidbody>();
            }

            if (fingerRb != null)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, fingerRb.position.z);
                aSource[0].volume = Mathf.Lerp(aSource[0].volume, 1, fingerRb.velocity.sqrMagnitude / 10);
                aSource[1].volume = Mathf.Lerp(aSource[1].volume, 1, fingerRb.velocity.sqrMagnitude / 10);
            }
        }
    }

    public void PlaySound()
    {
        aSource[0].volume = Mathf.Lerp(aSource[0].volume, 1, rb.velocity.sqrMagnitude / 10);
        aSource[1].volume = Mathf.Lerp(aSource[1].volume, 1, rb.velocity.sqrMagnitude / 10);
    }

    void OnCollisionExit(Collision other)
    {
        aSource[0].volume = 0;
        aSource[1].volume = 0;
        fingerRb = null;
    }


}
