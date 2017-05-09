using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
public class SurfaceAudioPlayer : MonoBehaviour {

    public List<AudioSource> aSource;

    public List<AudioMixerSnapshot> snapshots;

    public float changeThreshold = 0.5f;

    int currentSound = 0;

    [HideInInspector]
    public Rigidbody rb;
    Renderer rend;
    [System.Serializable]
    public class TestProperties
    {
        public float endThreshold = 0.05f;
        [HideInInspector]
        public float distToLeftFullCrossFade;
        [HideInInspector]
        public float distToRightFullCrossFade;
        [HideInInspector]
        public float distToThreshold;

        public float accuracy;
        public float normalizedAccuracy;
        public float elapsedTime = 0;
        public float intervalElapsedTime = 0;
        private int intervalAmounts = 0;
        public float distToEnd = 0;
        public float distToPath;
        public float pathWidth = 1;

        float error;
        float errorintervalElapsedTime;
        float errorElapsedTime;
        public float normalizedError;
        private float errorIntervalAmounts;

        public void LogAccuracy()
        {
            intervalElapsedTime += Time.deltaTime;
            elapsedTime += Time.deltaTime;
            if (intervalElapsedTime > 0.10f)
            {
                accuracy += distToPath;
                intervalAmounts++;
                //elapsedTime = 0;
            }
            
            normalizedAccuracy = accuracy / intervalAmounts;

            if(distToPath > pathWidth)
            {
                LogError();
            }
        }

      
        public void LogError()
        {
            errorintervalElapsedTime += Time.deltaTime;
            errorElapsedTime += Time.deltaTime;
            if (errorintervalElapsedTime > 0.10f)
            {
                error += distToPath;
                errorIntervalAmounts++;
            }
            normalizedError = error / errorIntervalAmounts;
        }
        public string GetAccuracy()
        {
            return distToThreshold.ToString();
        }

        public string GetLoggedData()
        {
            return elapsedTime.ToString() + "," + accuracy.ToString() + "," + normalizedAccuracy.ToString();
        }

        public void ResetProperties()
        {
            elapsedTime = 0;
            errorElapsedTime = 0;
            intervalAmounts = 0;
            errorIntervalAmounts = 0;
            accuracy = 0;
            error = 0;
            normalizedAccuracy = 0;
            normalizedError = 0;
        }
    }
    public TestProperties testProperties;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        startPos = transform.localPosition;
        Mute();
        if (myPath == null)
        RandomizePositionAndThreshold();
	}

    public float crossfadeRange = 0.1f;

    public void RandomizePositionAndThreshold()
    {
        changeThreshold = Random.Range(-0.80f, 0.80f);
        float startZPosition = Random.Range(-0.40f, 0.40f);
        float sign = Mathf.Sign(startZPosition) * 0.20f;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, changeThreshold + startZPosition + sign);
    }
    public void ResetCube()
    {
        testProperties.ResetProperties();
        transform.localPosition = startPos;
        Unlock();
        Mute();
    }


    public BezierSpline myPath;
    [HideInInspector]
    public bool selected = false;

    public void CrossFadeAudio()
    {
        if (!locked)
        {
            if (myPath != null)
            {
                PathCrossFade();
            }
            else
            {
                NoPathCrossFade();
            }
        }
    }

    private Vector3 startPos;
    void PathCrossFade()
    {
        Vector3 nearestPointOnSpline = myPath.GetNearestPoint(transform.position, 0.01f);


        ChangeColor(new Color(1,1,1) * ((1- Vector3.Distance(transform.position, nearestPointOnSpline)*5 * testProperties.pathWidth)));


        testProperties.distToPath = Vector3.Distance(transform.localPosition, transform.parent.InverseTransformPoint(nearestPointOnSpline))*5* testProperties.pathWidth;

        aSource[1].volume = 1 * testProperties.distToPath; 
        aSource[0].volume = 1 * (1- testProperties.distToPath);

        
        if((fingerRb != null || selected))
        {
            //RotateTowardsDirection();
            testProperties.LogAccuracy();
        }
        testProperties.distToEnd = Vector3.Distance(transform.position, myPath.transform.TransformPoint(myPath.GetControlPoint(myPath.ControlPointCount - 1)));


        if (testProperties.distToEnd < testProperties.endThreshold)
        {
            Debug.Log("Task Complete");
            Lock();
            ExecuteEvents.Execute<ICustomMessageTarget>(DataLogger.Instance.gameObject, null, (x, y) => x.LogData(this, (int)curTestType));

            //Lock Box
        }

    }
   
    void RotateTowardsDirection()
    {
        Vector3 directionOnSpline = myPath.GetNearestDirection(transform.position, 0.05f);

        Quaternion direction = Quaternion.LookRotation(directionOnSpline);
        transform.rotation = direction;
    }

    void NoPathCrossFade()
    {

        if (transform.localPosition.z < -1)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1);
        }
        if (transform.localPosition.z > 1)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1);
        }

        testProperties.distToThreshold = (transform.localPosition.z - changeThreshold);
        testProperties.distToLeftFullCrossFade = Mathf.Abs(transform.localPosition.z - changeThreshold - crossfadeRange) * 5;
        testProperties.distToRightFullCrossFade = Mathf.Abs(transform.localPosition.z - changeThreshold + crossfadeRange) * 5;

        if (transform.localPosition.z < changeThreshold + crossfadeRange && transform.localPosition.z > changeThreshold - crossfadeRange)
        {
            aSource[1].volume = 1 * testProperties.distToLeftFullCrossFade;
            aSource[0].volume = 1 * testProperties.distToRightFullCrossFade;
        }
        else if (testProperties.distToLeftFullCrossFade > testProperties.distToRightFullCrossFade)
        {
            aSource[0].volume = 0;
            aSource[1].volume = 1;
        }
        else
        {
            aSource[1].volume = 0;
            aSource[0].volume = 1;
        }

        if (transform.localPosition.z < -1)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1);
        }
        if (transform.localPosition.z > 1)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1);
        }
    }

    Rigidbody fingerRb;
    private Vector3 vel = new Vector3(0,0,0);
    Vector3 preVel = new Vector3(0, 0, 0);

    public PathIterator.TestType curTestType;

    void OnCollisionStay(Collision other)
    {
        if (other.transform.tag == "IndexTop" && !locked)
        {
            UnMute();
            if (fingerRb == null)
            {
                fingerRb = other.transform.GetComponent<Rigidbody>();
            }

            if (fingerRb != null)
            {
                selected = true;
                transform.position = new Vector3(startPos.x, fingerRb.position.y, fingerRb.position.z);
            }
        }
        else
        {
            selected = false;
        }
    }

    public void PlaySound()
    {
        aSource[0].volume = Mathf.Lerp(aSource[0].volume, 1, rb.velocity.sqrMagnitude / 10);
        aSource[1].volume = Mathf.Lerp(aSource[1].volume, 1, rb.velocity.sqrMagnitude / 10);
    }

    public void CrossFadeSound()
    {
        aSource[0].volume = Mathf.Lerp(aSource[0].volume, 1, testProperties.distToThreshold);
        aSource[1].volume = Mathf.Lerp(aSource[1].volume, 1, testProperties.distToThreshold);
    }
    void OnCollisionExit(Collision other)
    {
        Mute();
        fingerRb = null;
    }

    public void Mute()
    {
        aSource[0].mute = true;
        aSource[1].mute = true;
    }

    public void UnMute()
    {
        if (curTestType != PathIterator.TestType.Visual)
        {
            aSource[0].mute = false;
            aSource[1].mute = false;
        }
    }

    [HideInInspector]
    public bool locked = false;

    public void Lock()
    {
        ChangeColor(Color.black);
        locked = true;
    }

    public void Unlock()
    {
        ChangeColor(Color.white);
        locked = false;
    }

    void ChangeColor(Color col)
    {
        rend.material.SetColor("_Color", col);
    }

}
