using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFiller : MonoBehaviour {

    public GameObject Filler;
    public BezierSpline mySpline;
    // Use this for initialization
    List<GameObject> pathObjects;

	void Initialize () {
        mySpline = GetComponent<BezierSpline>();
        pathObjects = new List<GameObject>();
        float step = 0.001f;
        Quaternion temp = Quaternion.Euler(new Vector3(0, -90, 0));
        for (float i = 0; i < 1; i += step)
        {
            GameObject go = Instantiate(Filler, mySpline.GetPoint(i), temp, mySpline.transform);
            go.transform.localScale *= mySpline.pathWidth*2;
            pathObjects.Add(go);
        }
    }

    public void UpdatePath()
    {
        if(pathObjects == null)
        {
            Initialize();
        }

        float step = 0.001f;
        float curStep = 0;
        for (int i = 0; i < pathObjects.Count; i++)
        {
            pathObjects[i].SetActive(true);
            pathObjects[i].transform.localScale = Vector3.one * mySpline.pathWidth * 2;
            curStep += step;
            if(curStep >= mySpline.pathLengthModifier)
            {
                HideRest(i);
                break;
            }
        }
    }
	
    void HideRest(int start)
    {
        for (int i = start; i < pathObjects.Count; i++)
        {
            pathObjects[i].SetActive(false);
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
