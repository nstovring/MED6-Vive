﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMovement : MonoBehaviour {

	private bool movingUp = true;
	private int counter = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (movingUp) {
			//Debug.Log ("Object transform before movement: " + this.gameObject.transform.position);
			this.gameObject.transform.position += new Vector3 (0f, 0.005f, 0f);
			counter++;
			//Debug.Log ("Object transform after movement: " + this.gameObject.transform.position);
			if (counter == 50) {
				movingUp = !movingUp;
				counter = 0;
			}
		} else {
			//Debug.Log ("Object transform before movement: " + this.gameObject.transform.position);
			this.gameObject.transform.position += new Vector3 (0f, -0.005f, 0f);
			counter++;
			//Debug.Log ("Object transform after movement: " + this.gameObject.transform.position);
			if (counter == 50) {
				movingUp = !movingUp;
				counter = 0;
			}
		}
	}
}