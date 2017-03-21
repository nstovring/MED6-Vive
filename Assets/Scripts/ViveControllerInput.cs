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

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update () {
        //Input methods
        if (Controller.GetAxis() != Vector2.zero)
        {
            Debug.Log(gameObject.name + Controller.GetAxis());
        }

        if (Controller.GetHairTriggerDown())
        {
            Debug.Log(gameObject.name + " Trigger Press");
        }
        if (Controller.GetHairTrigger())
        {
            Debug.Log(gameObject.name + " Trigger Press");
        }

        if (Controller.GetHairTriggerUp())
        {
            //release object
            if (objectInHand)
            {
                ReleaseObject();
            }

            Debug.Log(gameObject.name + " Trigger Release");
        }

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
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

		//Test - pressing touch? Try 'touchpad' also instead of 'trigger'
		if (Controller.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
			//Move forward
			Vector2 value = Controller.GetAxis();

			if (value != Vector2.zero) { //If moving
				
			}
		}
    }//End Update

    //Picking up objects code
    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        collidingObject = col.gameObject;
    }
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

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
