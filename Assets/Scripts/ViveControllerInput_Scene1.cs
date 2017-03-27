using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerInput_Scene1 : MonoBehaviour {
    //Steam VR Variables
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller

    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    //Picking up object variables
    private GameObject collidingObject;
    private GameObject objectInHand;

	public GameObject animatedCharacter;

	public GameObject yellowButton;
	public GameObject purpleButton;
	public GameObject blueButton;
	public GameObject screen;
	public GameObject character; 

	public GameObject iPad;
	public GameObject sound;

	private bool moving = false;

	public Material outlinedMaterial;
	private Material savedMaterial;


    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update () {
        //Input methods

		if (moving) {
			//character.GetComponent <TestAnimationScene1> ().VSpeed = Input.GetAxis("Vertical");
			//Debug.Log ("vertical speed: " + Input.GetAxis ("Vertical"));
			character.GetComponent <TestAnimationScene1> ().VSpeed = 1;
		}


        if (Controller.GetAxis() != Vector2.zero)
        {
            //Debug.Log(gameObject.name + Controller.GetAxis());
        }

        if (Controller.GetHairTriggerDown())
        {
            //Debug.Log(gameObject.name + " Trigger Press");
        }
        if (Controller.GetHairTrigger())
        {
            //Debug.Log(gameObject.name + " Trigger Press");
        }

        if (Controller.GetHairTriggerUp())
        {
            //release object
            if (objectInHand)
            {
                ReleaseObject();
            }

            //Debug.Log(gameObject.name + " Trigger Release");
        }

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
			/**
            //grab object
            if (collidingObject)
            {
                GrabObject();
            }
            Debug.Log(gameObject.name + " Grip Press");*/
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Release");
        }
		if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			Debug.Log ("Touch pressed. Rotating and moving character");
			//Set camera rotation to move direction
			character.GetComponent <TestAnimationScene1> ().rotateCharacter();
			moving = true;
		}
		if (Controller.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad)) {
			//Debug.Log ("Touch released");
			moving = false;
			//character.GetComponent <TestAnimationScene1> ().VSpeed = Input.GetAxis("Vertical");
		}
		if (Controller.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
			//Debug.Log ("Collider detected: " + collidingObject.name);
			
			if (collidingObject == iPad) {
				Debug.Log ("iPad grabbed");
				//Animation first, then attach tablet
				animatedCharacter.GetComponent<TestAnimationScene1>().StartGrabTablet();
				GrabObject ();
			}

		}
    }//End Update

	public void vibrate(ushort time)
	{
		SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(time);
	}

    //Picking up objects code
    private void SetCollidingObject(Collider col)
    {
		/**
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }*/
        collidingObject = col.gameObject;
		//Debug.Log ("Collided object is now set: " + collidingObject.name);
    }
    public void OnTriggerEnter(Collider other)
    {
		Debug.Log ("Colliding object: " + other.name);
        SetCollidingObject(other);

		if (other.gameObject.tag == "outline") {
			savedMaterial = other.GetComponent<Renderer> ().material;
			//Debug.Log ("Saving material: " + savedMaterial);
			other.GetComponent<Renderer> ().material = outlinedMaterial;
		}
    }
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
		//Debug.Log ("Exiting object: " + other);

		if (other.gameObject.tag == "outline") {
			//Debug.Log ("Giving back old mat");
			other.GetComponent<Renderer> ().material = savedMaterial;
		}

        if (!collidingObject)
        {
            return;
        }
		//Debug.Log ("Collided object is now set to null");
        collidingObject = null;


    }
    private void GrabObject()
    {
        
        objectInHand = collidingObject;
        collidingObject = null;
        
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }
    private void ReleaseObject()
    {
        
        if (GetComponent<FixedJoint>())
        {
            
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        
        objectInHand = null;
    }//End Picking up objects
}
