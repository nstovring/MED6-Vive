using UnityEngine;
using System.Collections;

public class ScreenColor : MonoBehaviour {

	public int Color; 
	private float currentTime; 
	private float deltaTime; 

	void Update() {
		deltaTime = Time.unscaledTime - currentTime; 
		//print (deltaTime);

		if (deltaTime >= 1){
			Color = Random.Range(0,3);
			SwitchColor (Color);
		}
	}

	void SwitchColor (int color) {

		switch (color) {
		case 0:
		gameObject.GetComponent<Renderer> ().material.color = new Color(1,0,1,1);
		break;
		
		case 1:
		gameObject.GetComponent<Renderer> ().material.color = new Color(0,0,1,0);
		break;

		case 2:
		gameObject.GetComponent<Renderer> ().material.color = new Color(1, 1, 0, 1);
		break;

		default:
		break;
		}

		currentTime = Time.unscaledTime; 

	}
}
