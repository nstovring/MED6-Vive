using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{

    private Animator myAnimator;
    private float VSpeed;
    public float HSpeed;

    // Use this for initialization
    void Start ()
	{
	    myAnimator = GetComponent<Animator>();
	    myAnimator.applyRootMotion = true;

	}

    private bool sitting = false;
    private bool samba = false;

    // Update is called once per frame
    void Update ()
	{
	    VSpeed = Input.GetAxis("Vertical");
        HSpeed = Input.GetAxis("Horizontal");
        myAnimator.SetFloat("VSpeed",VSpeed);
        myAnimator.SetFloat("HSpeed", HSpeed);

	    if (Input.GetKeyUp(KeyCode.Space))
	    {
	        sitting = !sitting;
	        myAnimator.SetBool("Sitting", sitting);
	    }
        if (Input.GetKeyUp(KeyCode.M))
        {
            samba = !samba;
            myAnimator.SetBool("Samba", samba);
        }
    }
}
