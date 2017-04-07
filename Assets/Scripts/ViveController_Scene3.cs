using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController_Scene3 : MonoBehaviour {
    //Steam VR Variables
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller

    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    //Picking up object variables
    private GameObject collidingObject;
    private GameObject objectInHand;
	public GameObject classmate;
	public GameObject animatedCharacter;

	public GameObject verticalSpeed; 

	public List<Material> blendMaterials;
	public List<Material> objectMaterials;

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
                //ReleaseObject();
            }

            //Debug.Log(gameObject.name + " Trigger Release");
        }

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
		
        {
            //Debug.Log(gameObject.name + " Grip Press");
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Release");
        }
		if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			Debug.Log ("Touch pressed");
		}
		if (Controller.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
			Debug.Log ("pressing trigger");

			if (collidingObject == classmate) {
				Debug.Log ("Classmate clicked");
				//TODO: animation
				//animatedCharacter.GetComponent<Sound>().playCharacterResponse();
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
		Debug.Log ("Collided object is now set: " + collidingObject.name);
    }
    public void OnTriggerEnter(Collider other)
    {
		Debug.Log ("Colliding object: " + other.name);
        SetCollidingObject(other);

		//TODO: set tags + check names + insert new and old materials
		if (other.gameObject.tag == "outline") {
			other.GetComponent<Renderer> ().material = blendMaterials[0];
		} 
    }
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
		Debug.Log ("Exiting object");

		if (other.gameObject.tag == "outline") {
			//Debug.Log ("Giving back old mat");
			//other.GetComponent<Renderer> ().material = savedMaterial;
			if (other.gameObject.tag == "outline") {
				other.GetComponent<Renderer> ().material = objectMaterials[0];
			} 
			Debug.Log ("Object material: " + other.GetComponent<Renderer> ().material);
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
