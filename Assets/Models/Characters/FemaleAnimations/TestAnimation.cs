using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{

    private Animator myAnimator;
    private float VSpeed;
    public float HSpeed;

    public Transform handRoot;
    public List<Transform> tablets;

    public SteamVR_ControllerManager controllerMan;
    // Use this for initialization
    void Start ()
	{
	    myAnimator = GetComponent<Animator>();
	    myAnimator.applyRootMotion = true;
        if (controllerTest)
        {
            tablets[0].gameObject.SetActive(false);
            //tablets[0].gameObject.GetComponent<Collider>().enabled = false;
            //tablets[0].gameObject.GetComponent<Renderer>().enabled = false;
            //Destroy(tablets[0]);//.gameObject.GetComponent<Renderer>().enabled = false;


        } else {
            tablets[1].gameObject.SetActive(false);

            //tablets[1].gameObject.GetComponent<Collider>().enabled = true;
            //tablets[1].gameObject.GetComponent<Renderer>().enabled = true;
            //Destroy(tablets[1]);
        }
    }

    private bool sitting = false;
    private bool samba = false;
    public bool controllerTest = false;

    Vector3 tabPos = new Vector3(0.024f, 0.523f, 0.071f);
    Vector3 tabRot = new Vector3(80, -115, -287);
    public IEnumerator GrabTablet()
    {
        myAnimator.SetBool("GrabTablet", true);
        yield return new WaitForSeconds(1.5f);
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
        else
        {
            tablets[0].parent = handRoot;
            tablets[0].transform.localPosition = tabPos;
            Quaternion newRot = Quaternion.Euler(tabRot);
            tablets[0].transform.localRotation = newRot;
        }
        //foreach (Transform tablet in tablets) {
        //    tablet.parent = handRoot;
        //    tablet.transform.localPosition = tabPos;
        //    Quaternion newRot = Quaternion.Euler(tabRot);
        //    tablet.transform.localRotation = newRot;
        //}
    }


    //public IEnumerator WalkToTarget()
    //{
    //    while(Vector3)
    //}

    // Update is called once per frame
    void Update ()
	{
        myAnimator.SetBool("GrabTablet", false);
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

        if (Input.GetKeyUp(KeyCode.G))
        {
            StartCoroutine(GrabTablet());
        }
    }
}
