using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathIterator : MonoBehaviour {
    public List<BezierSpline> paths;
    private BezierSpline currentPath;
    public SurfaceAudioPlayer surfAudioPlayer;
    int pathIteration = 0;

    public enum TestType { Visual, VibroTactile, Both};

    public TestType myTestType = TestType.Visual;


    // Use this for initialization
    void Start()
    {
    }

    public void Initialize()
    {
        foreach (var item in paths)
        {
            item.transform.gameObject.SetActive(false);
        }
        IteratePath(DataLogger.Instance.taskVariables[0]);
        //(myTestType);
      
        //currentPath = paths[DataLogger.Instance.taskVariables[0].p];
        //currentPath.transform.gameObject.SetActive(true);
        //surfAudioPlayer.myPath = currentPath;
    }

    public void IteratePath()
    {
        paths[pathIteration].transform.gameObject.SetActive(false);
        pathIteration++;
        paths[pathIteration].transform.gameObject.SetActive(true);
        surfAudioPlayer.myPath = paths[pathIteration];
        surfAudioPlayer.ResetCube();
    }


    int initialTestType = 0;
    public void IterateInitialTest()
    {
        paths[0].transform.gameObject.SetActive(true);
        surfAudioPlayer.myPath = paths[0];
        SwitchTestType((TestType)initialTestType);
        surfAudioPlayer.ResetCube();
        initialTestType++;
    }
    public void IteratePath(int testProgression)
    {
        currentPath.transform.gameObject.SetActive(false);
        currentPath = paths[testProgression];
        currentPath.transform.gameObject.SetActive(true);
        surfAudioPlayer.ResetCube();
        surfAudioPlayer.myPath = currentPath;
    }

    public void IteratePath(DataLogger.TaskVariables testVariables)
    {
        SwitchTestType((TestType)testVariables.testCondition);
        if(currentPath != null)
        currentPath.transform.gameObject.SetActive(false);
        currentPath = paths[testVariables.pathType];
        currentPath.transform.gameObject.SetActive(true);
        surfAudioPlayer.testProperties.data.logThisData = testVariables.logThisData;
        //surfAudioPlayer.ResetCube();
        surfAudioPlayer.myPath = currentPath;
        SetPathWidth(testVariables.pathWidth);
        SetPathLength(testVariables.pathLength, testVariables.pathType);
    }
    public void SwitchTestType(TestType _myTestType)
    {
        myTestType = _myTestType;
        switch (_myTestType)
        {
            case TestType.Visual:
                SetVisuals(true);
                SetVibroTactileRespone(false);
                break;
            case TestType.VibroTactile:
                SetVisuals(false);
                SetVibroTactileRespone(true);
                break;
            case TestType.Both:
                SetVisuals(true);
                SetVibroTactileRespone(true);
                break;
        }

        surfAudioPlayer.testProperties.curTestType = _myTestType;
    }

    void SetPathWidth(float pathWidth)
    {
        currentPath.pathWidth = pathWidth/5;
        surfAudioPlayer.testProperties.pathWidth = pathWidth;
    }

    void SetPathLength(float multiplier, int pathType)
    {
        float pathLength = currentPath.CalculateSplineLength() * multiplier;
        currentPath.pathLengthModifier = multiplier;
        surfAudioPlayer.testProperties.pathLength = pathLength;
        surfAudioPlayer.testProperties.pathModifier = multiplier;
        surfAudioPlayer.testProperties.pathType = pathType;
    }

    void SetVisuals(bool state)
    {
        foreach (var item in paths)
        {
            item.DrawGizmos = state;
        }
    }

    void SetVibroTactileRespone(bool state)
    {
        if(state == false)
        {
            surfAudioPlayer.Mute();
        }
        else
        {
            surfAudioPlayer.UnMute();
        }
    }

    void NextPath()
    {

    }
}
