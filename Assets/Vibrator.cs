using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrator : MonoBehaviour {
    SteamVR_ControllerManager controllerMan;
    Collider controllerCollider;
    private SteamVR_TrackedObject trackedObj;
    // Use this for initialization
    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update () {
        
	}
    public int mainFreq = 500;
    public int timeFreq = 10;
    void OnTriggerStay(Collider other) {
		/**
        if(other.transform.tag == "Tablet")
        {
			Debug.Log ("Ipad touched");
            //ushort time = (ushort)(int) (mainFreq * Mathf.Sin(Time.time * timeFreq));
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(500);

        }*/
    }

    public void vibrate(ushort time)
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(time);
    }
}
