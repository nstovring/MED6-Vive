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


    public float crossfadeRange = 0.1f;

    public BezierSpline myPath;
    [HideInInspector]
    public bool selected = false;
    private Vector3 startPos;


    [HideInInspector]
    public Rigidbody rb;
    Renderer rend;


    Rigidbody fingerRb;
    private Vector3 vel = new Vector3(0, 0, 0);
    Vector3 preVel = new Vector3(0, 0, 0);

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
        public float pathLength;
        public float pathModifier;

        float error;
        float errorintervalElapsedTime;
        float errorElapsedTime;
        public float normalizedError;
        private float errorIntervalAmounts;

        public int pathType;
        public DataLogged data;
        public PathIterator.TestType curTestType;

        public void LogAccuracy()
        {
            intervalElapsedTime += Time.deltaTime;
            elapsedTime += Time.deltaTime;
            if (intervalElapsedTime > 0.10f)
            {
                accuracy += distToPath;
                intervalAmounts++;
            }
            
            normalizedAccuracy = accuracy / intervalAmounts;
            data.accuracy = normalizedAccuracy;
            data.elapsedTime = elapsedTime;
            data.ID = Mathf.Log(((2*pathLength) / pathWidth)+1,2);
            data.pathType = pathType;
            data.testCondiction = (int)curTestType;
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
	public void Initialize () {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        startPos = transform.localPosition;
        Mute();
        if (myPath == null)
        RandomizePositionAndThreshold();
	}

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
        Initialize();
    }


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
    
    void PathCrossFade()
    {
        Vector3 nearestPointOnSpline = myPath.GetNearestPoint(transform.position, 0.001f);

        if(testProperties.curTestType != PathIterator.TestType.VibroTactile)
        ChangeColor(new Color(1,1,1) * ((1- Vector3.Distance(transform.position, nearestPointOnSpline)*10 /testProperties.pathWidth)));


        testProperties.distToPath = Vector3.Distance(transform.localPosition, transform.parent.InverseTransformPoint(nearestPointOnSpline))*10/ testProperties.pathWidth;

        aSource[0].volume = 1 * testProperties.distToPath; 
        aSource[1].volume = 1 * (1- testProperties.distToPath);

        
        if((fingerRb != null || selected))
        {
            //RotateTowardsDirection();
            testProperties.LogAccuracy();
        }
        Vector3 endPoint = myPath.GetPoint(1 * testProperties.pathModifier);
        testProperties.distToEnd =Vector3.Distance(transform.position, endPoint);

        if ((testProperties.distToEnd < testProperties.endThreshold ))
        {
            Debug.Log("Task Complete");
            Lock();
            if (testProperties.data.logThisData)
            {
                Debug.Log("Logging Get New path");
                ExecuteEvents.Execute<ICustomMessageTarget>(DataLogger.Instance.gameObject, null, (x, y) => x.LogData(testProperties.data));
            }
            else
            {
                Debug.Log("Not Logging Get New path");
                ExecuteEvents.Execute<ICustomMessageTarget>(DataLogger.Instance.gameObject, null, (x, y) => x.GetRandomNewPath());
            }
            ResetCube();
        }

        if(testProperties.distToPath > 1)
        {
            Lock();
            testProperties.data.accuracy = -1;
            testProperties.data.elapsedTime = -1;
            if (testProperties.data.logThisData)
            {
                Debug.Log("Error Logged");
                ExecuteEvents.Execute<ICustomMessageTarget>(DataLogger.Instance.gameObject, null, (x, y) => x.LogData(testProperties.data));
            }
            else
            {
                Debug.Log("Not Logging Get New path");
                ExecuteEvents.Execute<ICustomMessageTarget>(DataLogger.Instance.gameObject, null, (x, y) => x.GetRandomNewPath());
            }
            ResetCube();
        }

    }
    [System.Serializable]
    public struct DataLogged
    {
        public int pathType;
        public float pathWidth;
        public float ID;
        public float elapsedTime;
        public float accuracy;
        public int testCondiction;
        public bool logThisData;
        public string GetData()
        {
            return pathType.ToString() + "\t" + ID.ToString() + "\t" + accuracy.ToString() + "\t" + elapsedTime.ToString();
        }

        public string GetError()
        {
            return pathType.ToString() + "\t" + ID.ToString() + "\t" + testCondiction.ToString();
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
        if (testProperties.curTestType != PathIterator.TestType.Visual)
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
