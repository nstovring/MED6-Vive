using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.SceneManagement;

public class TestAnimationScene1 : MonoBehaviour
{
	//iPad reference
	public GameObject iPad;
	//public GameObject iPadScreen;

    private Animator myAnimator;
    public float VSpeed;
    public float HSpeed;

    public Transform handRoot;
	public List<Transform> tablets;

	public SteamVR_ControllerManager controllerMan;

	private VRCameraFade myFade;

	private Material[] materials;
	private int currentMat = 0;
	private Transform ipadScreen;

	public Material iPadMat;
	public Material blendMat;

	private Sound soundScript;

	public Vector3 offset = new Vector3(-0.2f, 0.05f, 0f);

	public List<Material> diffuseMaps;
	public List<Texture2D> textures;

	public GameObject characterMesh;


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

    private bool sitting = false;
    private bool samba = false;
    public bool controllerTest = false;

    Vector3 tabPos = new Vector3(0.024f, 0.523f, 0.071f);
    Vector3 tabRot = new Vector3(80, -115, -287);

	public IEnumerator faceAnimations(int numberOfAnimations) {
		//Transform child = this.gameObject.transform.GetChild (1);
		//Debug.Log ("Child found: " + child.name);

		while (numberOfAnimations > 0) {
			//Change between the two facial maps
			Debug.Log ("Iteration no. " + (11 - numberOfAnimations) + ". Applying material " + (numberOfAnimations % 2));
			//child.GetComponent<Renderer> ().material = diffuseMaps [numberOfAnimations % 2];
			characterMesh.GetComponent<Renderer> ().material = diffuseMaps [(numberOfAnimations + 1) % 2];
			yield return new WaitForSeconds (0.2f);
			numberOfAnimations--;
		}
	}

	public void StartGrabTablet(Transform root) {
		StartCoroutine(GrabTablet(root));
	}

	private IEnumerator GrabTablet(Transform root)
    {
		Debug.Log ("Grabbing tablet");
        //myAnimator.SetBool("GrabTablet", true);
        //yield return new WaitForSeconds(1.5f);
		/**
        if (controllerTest == true)
        {
            tablets[1].parent = handRoot;
            tablets[1].transform.localPosition = tabPos;
            Quaternion newRot = Quaternion.Euler(tabRot);
            tablets[1].transform.localRotation = newRot;

            int time = 100;
        while (time > 0)
        	{
            controllerMan.left.GetComponent<Vibrator>().vibrate(1000);
            controllerMan.right.GetComponent<Vibrator>().vibrate(1000);
            time--;
            yield return new WaitForSeconds(0.01f);
        	}
        }
        else*/
		if(true)
        {
			//Setting the parent of the talet to be the hand root
			//TODO: set to be controller instead of hand root
            //tablets[0].parent = handRoot;     
			tablets[0].parent = root;
			//tablets[0].transform.localPosition = tabPos;
			tablets[0].transform.position = root.position + offset;
			//tablets[0].transform.localPosition = iPad.transform.position;
            //Quaternion newRot = Quaternion.Euler(tabRot);
			//Quaternion newRot = Quaternion.Euler(root.rotation);
			tablets[0].transform.localRotation = root.rotation;
        }
		yield return new WaitForSeconds(0);
		//Girl looks up and says "stop it"
		soundScript.playAudio(soundScript.narrationResponse[0]); //Play sound
		StartCoroutine (faceAnimations (5)); //Animate face

        //foreach (Transform tablet in tablets) {
        //    tablet.parent = handRoot;
        //    tablet.transform.localPosition = tabPos;
        //    Quaternion newRot = Quaternion.Euler(tabRot);
        //    tablet.transform.localRotation = newRot;
        //}
    }


    public IEnumerator WalkToTarget()
    {
        //while(Vector3)

		yield return new WaitForSeconds(0.01f);
    }

	//Make the character rotate along with the camera
	public void rotateCharacter(){
		var cameraRotation = Camera.main.transform.rotation;
		cameraRotation.x = 0;
		cameraRotation.z = 0;
		this.transform.rotation = cameraRotation;
	}

    // Update is called once per frame
    void Update ()
	{
		if(Input.GetKey(KeyCode.I)) {
			Transform child = this.gameObject.transform.GetChild (1);
			Debug.Log ("Child found: " + child.name);

			//Change between the two facial maps
			//child.GetComponent<Renderer> ().material = diffuseMaps [0];
			characterMesh.GetComponent<Renderer> ().material = diffuseMaps [0];
		}

		if(Input.GetKey(KeyCode.O)) {
			Transform child = this.gameObject.transform.GetChild (1);
			//Debug.Log ("Child found: " + child.name + ". Applying diffuse map: " + diffuseMaps [1].name);

			//Change between the two facial maps
			characterMesh.GetComponent<Renderer> ().material = diffuseMaps [1];
		}


        myAnimator.SetBool("GrabTablet", false);
		/**
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            HSpeed = Input.GetAxis("Horizontal");
			//this.transform.Rotate
			//rotateCharacter();
			//Debug.Log ("Horizontal speed: " + Input.GetAxis ("Horizontal"));
        }*/

		//Debug.Log ("Vertical speed: " + VSpeed);
       
        myAnimator.SetFloat("VSpeed",VSpeed);
        myAnimator.SetFloat("HSpeed", HSpeed);


        VSpeed = Mathf.Lerp(VSpeed, 0,0.1f);
        HSpeed = Mathf.Lerp(HSpeed, 0, 0.1f);

		/**
	    if (Input.GetKeyUp(KeyCode.Space))
	    {
	        sitting = !sitting;
	        myAnimator.SetBool("Sitting", sitting);
	    }*/

        if (Input.GetKeyUp(KeyCode.G))
        {
			StartCoroutine(GrabTablet(handRoot));
        }

		//Load menu
		if (Input.GetKeyUp (KeyCode.M)) {
			Debug.Log ("Fading and changing scene");
			myFade.FadeOut(2, false);
			StartCoroutine(waitAndLoad (2, "Menu"));
		}
		//Restart/reload scene
		if (Input.GetKeyUp (KeyCode.R)) {
			Debug.Log ("Fading and restarting scene");
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




		if (Input.GetKeyUp (KeyCode.T)) {
			//DynamicGI.SetEmissive (iPad.GetComponent<Renderer> (), new Color(255,255,255));
				//iPad.GetComponent<Renderer>().material.color + new Color(0.5f, 0.5f, 0.5f));
			iPad.GetComponent<Renderer> ().material = iPadMat;
			Debug.Log ("Object color: " + iPad.GetComponent<Renderer> ().material);
		}
		if (Input.GetKeyUp (KeyCode.Y)) {
			//DynamicGI.SetEmissive (iPad.GetComponent<Renderer> (), new Color(255,255,255));
			//iPad.GetComponent<Renderer>().material.color + new Color(0.5f, 0.5f, 0.5f));
			iPad.GetComponent<Renderer> ().material = blendMat;
			Debug.Log ("Object color: " + iPad.GetComponent<Renderer> ().material);
		}

		/**
		if (Input.GetKeyUp(KeyCode.T))
		{
			Debug.Log ("Changing material on tablet");


			if (currentMat == 0) {
				Debug.Log ("First material");
				ipadScreen.GetComponent<Renderer> ().sharedMaterial = materials [1];
				currentMat = 1;
			} else {
				Debug.Log ("Second material");
				ipadScreen.GetComponent<Renderer> ().sharedMaterial = materials [0];
				currentMat = 0;
			}
		}*/

		//Clicking iPad
		/**
		clickedDown = controller.GetPressDown(clickedButton);

		if (clickedDown) {

			Debug.Log("Controller clicked");
			//public Renderer rend;
			//start:
			//rend = GetComponent<Renderer>()
			//rend.enabled = true;
			//update:
			//rend.sharedMaterial = materials[index];

			if (currentMat == 0) {
				Debug.Log ("First material");
				ipadScreen.GetComponent<Renderer> ().sharedMaterial = materials [1];
				//ipadScreen.GetComponent<Renderer> ().sharedMaterial.color = Color.blue;
				currentMat = 1;
			} else {
				Debug.Log ("Second material");
				ipadScreen.GetComponent<Renderer> ().sharedMaterial = materials [0];
				//ipadScreen.GetComponent<Renderer> ().sharedMaterial.color = Color.white;
				currentMat = 0;
			}
		}*/

    }

	IEnumerator waitAndLoad(int seconds, string scene) {
		Debug.Log ("Waiting before load");
		yield return new WaitForSeconds(seconds);
		Debug.Log ("Loading");
		SceneManager.LoadScene(scene);
	}
}
