using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class FadeScript : MonoBehaviour {

	private VRCameraFade myFade;

	// Use this for initialization
	void Start () {
		new WaitForSeconds(2);
		myFade = Camera.main.GetComponent<VRCameraFade>();
		myFade.FadeIn(2, false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
