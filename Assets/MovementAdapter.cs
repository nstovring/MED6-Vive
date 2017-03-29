using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
public class MovementAdapter : MonoBehaviour {

    public float rightHandDelta;
    public float leftHandDelta;

    public RigidHand leftHand;
    public RigidHand rightHand;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int boneCount = rightHand.fingers[1].bones.Length;

        rightHandDelta = rightHand.fingers[1].bones[boneCount - 1].GetComponent<Rigidbody>().velocity.sqrMagnitude;

        boneCount = leftHand.fingers[1].bones.Length;
        leftHandDelta = leftHand.fingers[1].bones[boneCount - 1].GetComponent<Rigidbody>().velocity.sqrMagnitude;

    }
}
