using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRigStabilizer : MonoBehaviour {

	private Vector3 InitialPos;
	private Vector3 hmdPos;
	public GameObject HMD;
	public Transform CameraPos;

	void Start ()
	{
		InitialPos = transform.position;
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		//hmdPos = HMD.transform.position;                         Can't work in world position
		//transform.position = InitialPos - hmdPos;                for some reason
		hmdPos = HMD.transform.localPosition;
		//Debug.Log ("hmdPos: " + hmdPos);
		transform.position = CameraPos.position - hmdPos;
		//Debug.Log ("End pos: " + (CameraPos.position - hmdPos));
		//this.transform.position = CameraPos.position;
		//Debug.Log ("Camera pos: " + (CameraPos.position));
		//Debug.Log ("End pos: " + (this.transform.position));

	}

}
