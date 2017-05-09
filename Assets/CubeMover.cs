using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour {
    public bool enabled = true;
	// Use this for initialization
	void Start () {
		
	}
    SurfaceAudioPlayer selectedCube;

    public Vector3 mouseposition;
	// Update is called once per frame
	void Update () {
      
	}

    public void SelectCube()
    {
        if (enabled)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Cube")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        selectedCube = hit.transform.GetComponent<SurfaceAudioPlayer>();
                    }
                }
                else if (selectedCube != null)
                {
                    selectedCube.Mute();
                    selectedCube.selected = false;
                    selectedCube = null;
                }
            }

            if (selectedCube != null && !selectedCube.locked)
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseposition = selectedCube.transform.parent.transform.InverseTransformPoint(mouseWorldPosition);
                selectedCube.UnMute();
                selectedCube.selected = true;

                selectedCube.transform.localPosition = new Vector3(selectedCube.transform.localPosition.x, mouseposition.y, mouseposition.z);
                if (Input.GetMouseButtonUp(0))
                {
                    selectedCube.Mute();
                    selectedCube.selected = false;
                    selectedCube = null;
                }
            }
        }
    }
}
