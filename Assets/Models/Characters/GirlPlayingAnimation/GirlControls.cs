using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlControls : MonoBehaviour {

	//static variable. Is an animator
	//Goes back and get the controller, which is attatched to the girl
	static Animator anim;

	//public Transform upperBody;

	// Use this for initialization
	void Start () {

		//setting the animator control into the anim
		//Is used to set the trigger
		anim = GetComponent<Animator> ();

		//Animation ["button_pushing"].layer = 5;
		//Animation ["button_pushing"].AddMixingTransform (upperBody);

	}
	
	// Update is called once per frame
	void Update () {
		

		if (Input.GetKey(KeyCode.X)) {


			anim.SetTrigger("isButtonPressingR");
		}

		if (Input.GetKey(KeyCode.C)) {


			anim.SetTrigger("isButtonPressingM");
		}
	}
}
