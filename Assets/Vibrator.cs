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
        controllerMan = GetComponent<SteamVR_ControllerManager>();
        controllerCollider = controllerMan.left.GetComponent<Collider>();

    }

    // Update is called once per frame
    void Update () {
        
	}

    void OnTriggerStay(Collider other) {
        if(other.transform.tag == "Tablet")
        {
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(500);

        }
    }
}
