using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {
	AudioClip ambientSound = Resources.Load<AudioClip>("Sounds/External_Hope");
	AudioClip narrative = Resources.Load<AudioClip>("Sounds/Correct");
	//AudioClip correctColour = Resources.Load<AudioClip>("Sounds/External_Hope");
	//AudioClip incorrectColour = Resources.Load<AudioClip>("Sounds/External_Hope");


	public GameObject ambient; 

	public void SwitchSound (float sound) {
		
	switch (sound) {
	case 0:				
	case 1:
	break;

	}
}
}