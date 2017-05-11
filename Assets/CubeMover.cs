using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour {
    public bool enabledScript = true;
	// Use this for initialization
	void Start () {
		
	}
    [HideInInspector]
    public SurfaceAudioPlayer selectedCube;

    public Vector3 mouseposition;
	// Update is called once per frame
	void Update () {
      
	}
    Vector3 mouseOffset;
    public void SelectCube()
    {

        if (enabledScript)
        {
            if (Input.GetMouseButtonDown(0))
            {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Cube")
                {
                    
                        selectedCube = hit.transform.GetComponent<SurfaceAudioPlayer>();
                        mouseOffset = selectedCube.transform.localPosition - selectedCube.transform.parent.transform.InverseTransformPoint(new Vector3(0,hit.point.y, hit.point.z)) ;
                        selectedCube.selected = true;
                    }
                }
                //else 
                //{
                //    selectedCube.Mute();
                //    selectedCube.selected = false;
                //    selectedCube = null;
                //}
            }

            if(selectedCube != null && selectedCube.selected == false)
            {
                selectedCube.ResetCube();
                selectedCube.Mute();
                selectedCube.selected = false;
                selectedCube = null;
            }

            if (selectedCube != null && !selectedCube.locked && selectedCube.selected == true)
            {
                //Vector3 tempVector = new Vector3(selectedCube.transform.localPosition.x, Input.mousePosition.y, Input.mousePosition.z);
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseposition = selectedCube.transform.parent.transform.InverseTransformPoint(mouseWorldPosition);
                selectedCube.UnMute();

                selectedCube.transform.localPosition = new Vector3(selectedCube.transform.localPosition.x, mouseposition.y + mouseOffset.y, mouseposition.z+ mouseOffset.z);
                if (Input.GetMouseButtonUp(0) || selectedCube.selected == false)
                {
                    selectedCube.Mute();
                    selectedCube.selected = false;
                    selectedCube = null;
                }
            }
        }
    }
}
