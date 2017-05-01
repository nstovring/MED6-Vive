using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour {
    public bool enabled = true;
	// Use this for initialization
	void Start () {
		
	}
    SurfaceAudioPlayer selectedCube;
	// Update is called once per frame
	void Update () {
        if (enabled)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit))
            {
                if(hit.transform.tag == "Cube")
                {
                    selectedCube = hit.transform.GetComponent<SurfaceAudioPlayer>();
                }
            }

            if(selectedCube != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    selectedCube.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, mouseWorldPosition.z);

                }
            }
        }
	}
}
