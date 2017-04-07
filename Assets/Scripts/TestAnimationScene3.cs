﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.SceneManagement;

public class TestAnimationScene3 : MonoBehaviour
{
	//iPad reference
	public GameObject iPad;
	//public GameObject iPadScreen;

    private Animator myAnimator;
    public float VSpeed;
    public float HSpeed;

	public List<Transform> tablets;

	public SteamVR_ControllerManager controllerMan;

	private VRCameraFade myFade;

	private Material[] materials;
	private int currentMat = 0;
	private Transform ipadScreen;

	public Material iPadMat;
	public Material blendMat;

	private Sound soundScript;

	public List<Material> diffuseMaps;


	void Awake()
	{
		soundScript = this.GetComponent<Sound> ();
	}

    // Use this for initialization
    void Start ()
	{
		myFade = Camera.main.GetComponent<VRCameraFade>();

		//Instantiate iPad screen color
		ipadScreen = iPad.gameObject.transform.GetChild (0);
		ipadScreen.GetComponent<Renderer> ().sharedMaterial.color = Color.white;

		materials = new Material[2];
		//Debug.Log ("Array materials length: " + materials.Length);
		materials [0] = (Material)Resources.Load ("White", typeof(Material)) as Material;
		materials [1] = (Material)Resources.Load ("Test", typeof(Material))  as Material;

	    myAnimator = GetComponent<Animator>();
		//Debug.Log("MyAnimator result: " + myAnimator);
	    myAnimator.applyRootMotion = true;
    }

	public IEnumerator faceAnimations(int numberOfAnimations) {
		while (numberOfAnimations > 0) {
			//Change between the two facial maps
			this.GetComponent<Renderer> ().material = diffuseMaps [0 % 2];
			yield return new WaitForSeconds (0.2);
			numberOfAnimations--;
		}
	}

    // Update is called once per frame
    void Update ()
	{

        myAnimator.SetBool("GrabTablet", false);
       
        myAnimator.SetFloat("VSpeed",VSpeed);
        myAnimator.SetFloat("HSpeed", HSpeed);


        VSpeed = Mathf.Lerp(VSpeed, 0,0.1f);
        HSpeed = Mathf.Lerp(HSpeed, 0, 0.1f);

		//Load menu
		if (Input.GetKeyUp (KeyCode.M)) {
			Debug.Log ("Fading and changing scene");
			myFade.FadeOut(2, false);
			StartCoroutine(waitAndLoad (2, "Menu"));
		}
		//Restart/reload scene
		else if (Input.GetKeyUp (KeyCode.R)) {
			Debug.Log ("Fading and restarting scene");
			myFade.FadeOut(1, false);
			StartCoroutine(waitAndLoad (1, "Scene1"));
		}
		//For teacher
		else if (Input.GetKeyUp (KeyCode.Alpha1)) {
			Debug.Log ("Yes response");
			soundScript.playAudio(soundScript.narrationResponse[0]); //Play sound
		}
		else if (Input.GetKeyUp (KeyCode.Alpha2)) {
			Debug.Log ("No response");
			soundScript.playAudio(soundScript.narrationResponse[1]); //Play sound
		}

    }

	IEnumerator waitAndLoad(int seconds, string scene) {
		Debug.Log ("Waiting before load");
		yield return new WaitForSeconds(seconds);
		Debug.Log ("Loading");
		SceneManager.LoadScene(scene);
	}
}
