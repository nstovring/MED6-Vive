using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerInput : MonoBehaviour {
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
	public GameObject verticalSpeed; 
	public GameObject sounds; 


	public int mainFreq = 500;
	public int timeFreq = 10;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update () {
        //Input methods


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
			verticalSpeed.GetComponent <TestAnimation1> ().VSpeed = Input.GetAxis("Vertical");
		
        {


            //grab object
            if (collidingObject)
            {
                GrabObject();
            }



            Debug.Log(gameObject.name + " Grip Press");
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Release");
        }
		if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			Debug.Log ("Touch pressed");
		}
		if (Controller.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {

			if (collidingObject == yellowButton) {
				Debug.Log ("Yellow button pressed");

				//If color is correct
				if (screen.GetComponent<Renderer> ().material.color == new Color (1, 1, 0)) {
					Debug.Log ("Correct color chosen");
					screen.GetComponent <Renderer> ().material.color = new Color (0, 1, 0);
					//sounds.GetComponent<Sound> ().ipadSounds [0]; 

				} else {
					Debug.Log ("Wrong color chosen");
					screen.GetComponent <Renderer> ().material.color = new Color (1, 0, 0); 
					vibrate (500);
				}
				screen.GetComponent<ScreenColor>().playing = false;
			} 
			else if (collidingObject == purpleButton) {
				Debug.Log("Purple button pressed");

				if (screen.GetComponent<Renderer> ().material.color == new Color (1, 0, 1)) {
					Debug.Log ("Correct color chosen");
					screen.GetComponent <Renderer> ().material.color = new Color (0,1,0);


				} else {
					Debug.Log ("Wrong color chosen");
					screen.GetComponent <Renderer> ().material.color = new Color (1,0,0); 
					vibrate (500);
				}
				screen.GetComponent<ScreenColor>().playing = false;
			}
			else if (collidingObject == blueButton) {
				Debug.Log("Blue button pressed");

				if (screen.GetComponent<Renderer> ().material.color == new Color (0, 0, 1)) {
					Debug.Log ("Correct color chosen");
					screen.GetComponent <Renderer> ().material.color = new Color (0,1,0); 


				} else {
					Debug.Log ("Wrong color chosen");
					screen.GetComponent <Renderer> ().material.color = new Color (1,0,0); 
					vibrate (500);
				}
				screen.GetComponent<ScreenColor>().playing = false;
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
		//Debug.Log ("Colliding object: " + other.name);
        SetCollidingObject(other);
    }
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
		//Debug.Log ("Exiting object");
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
