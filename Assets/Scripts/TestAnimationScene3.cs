using System.Collections;
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

	private Sound soundScript;

	public List<Material> diffuseMaps;
	public GameObject characterMesh;

	public GameObject iPadGroup;

	private bool rotating = false;
	private int rotations = 0;


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
			Debug.Log ("Iteration no. " + (11 - numberOfAnimations) + ". Applying material " + (numberOfAnimations % 2));
			characterMesh.GetComponent<Renderer> ().material = diffuseMaps [(numberOfAnimations + 1) % 2];
			yield return new WaitForSeconds (0.2f);
			numberOfAnimations--;
		}
	}

    // Update is called once per frame
    void Update ()
	{

        //myAnimator.SetBool("GrabTablet", false);
       
        //myAnimator.SetFloat("VSpeed",VSpeed);
        //myAnimator.SetFloat("HSpeed", HSpeed);


        //VSpeed = Mathf.Lerp(VSpeed, 0,0.1f);
        //HSpeed = Mathf.Lerp(HSpeed, 0, 0.1f);

		//Load menu
		if (Input.GetKeyUp (KeyCode.M)) {
			Debug.Log ("Fading and changing scene");
			myFade.FadeOut(2, false);
			StartCoroutine(waitAndLoad (2, "Menu"));
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			Debug.Log ("Fading and changing scene");
			myFade.FadeOut(2, false);
			StartCoroutine(waitAndLoad (2, "Menu"));
		}

		//Restart/reload scene
		else if (Input.GetKeyUp (KeyCode.R)) {
			Debug.Log ("Fading and restarting scene");
			myFade.FadeOut(1, false);
			StartCoroutine(waitAndLoad (1, "Scene3"));
		}
		else if (Input.GetKeyUp (KeyCode.Return)) {
			Debug.Log ("Fading and restarting scene");
			myFade.FadeOut(1, false);
			StartCoroutine(waitAndLoad (1, "Scene3"));
		}

		if (Input.GetKeyUp (KeyCode.Alpha1)) {
			Debug.Log ("Fading and changing to scene 1");
			myFade.FadeOut(1, false);
			StartCoroutine(waitAndLoad (1, "Scene1"));
		}
		if (Input.GetKeyUp (KeyCode.Alpha2)) {
			Debug.Log ("Fading and changing to scene 2");
			myFade.FadeOut(1, false);
			StartCoroutine(waitAndLoad (1, "Scene2"));
		}
		if (Input.GetKeyUp (KeyCode.Alpha3)) {
			Debug.Log ("Fading and changing to scene 3");
			myFade.FadeOut(1, false);
			StartCoroutine(waitAndLoad (1, "Scene3"));
		}

		//For teacher
		else if (Input.GetKeyUp (KeyCode.J)) {
			Debug.Log ("Yes response");
			soundScript.playAudio(soundScript.narrationResponse[0]); //Play sound
			StartCoroutine (faceAnimations (10)); //Animate face
		}
		else if (Input.GetKeyUp (KeyCode.N)) {
			Debug.Log ("No response");
			soundScript.playAudio(soundScript.narrationResponse[1]); //Play sound
			StartCoroutine (faceAnimations (10)); //Animate face
		}

		else if (Input.GetKeyUp (KeyCode.T)) {
			Debug.Log ("Rotating and pushing tablet");
			//TODO: group tablet and buttons, and have reference to the parent. Rotate parent
			//Initiate pushing animation
			myAnimator.SetTrigger("PushingTrigger");
			rotating = true;
		}

		if (rotating) {
			if (rotations < 80) {
				//Debug.Log ("Rotating");
				iPadGroup.transform.Rotate (90 * Vector3.up * Time.deltaTime);
				rotations++;
			} else {
				rotating = false;
			}

		}


    }

	IEnumerator waitAndLoad(int seconds, string scene) {
		Debug.Log ("Waiting before load");
		yield return new WaitForSeconds(seconds);
		Debug.Log ("Loading");
		SceneManager.LoadScene(scene);
	}
}
