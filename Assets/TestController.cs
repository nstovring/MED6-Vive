using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
//Class which controls when behaviour occurs during test
//Mainly controls when classes update
//Maybe UI Stuff
public class TestController : MonoBehaviour, ICustomMessageTarget
{
    public PathIterator pathIterator;
    public DataLogger dataLogger;
    public CubeMover cubeMover;
    public SurfaceAudioPlayer cube;

    public Text testTypeText;
    public GameObject testTypeSwitchUI;
    public Text testTypeSwitchUIText;
    public bool tightThreshold = false;

    public static TestController Instance;

	// Use this for initialization
	void Start () {
        Instance = this;
        dataLogger.Initialize();
        pathIterator.Initialize();
        cube.Initialize();
	}
	
	// Update is called once per frame
	void Update () {

       
	}

    void LateUpdate()
    {
        cubeMover.SelectCube();
        cube.CrossFadeAudio();
        UpdateUI();
    }

    [MenuItem("Test Menu/Reset Test")]
    static void ResetTest()
    {
        DataLogger dl = DataLogger.Instance;
        dl.Initialize();
        dl.pathIterator.Initialize();
        dl.pathIterator.surfAudioPlayer.Initialize();
    }

    void UpdateUI()
    {
        testTypeText.text = " Test Progression: " + ((dataLogger.testProgression)% (dataLogger.taskVariables.Count/4)+1) + "/" + dataLogger.testAmount/3+ " of Test Type: "+ DataLogger.fileNames[(int)pathIterator.myTestType] + ", Path Type: " + DataLogger.pathNames[dataLogger.taskVariables[dataLogger.testProgression].pathType];
    }

    public void LogData(SurfaceAudioPlayer cube, int testType)
    {
        throw new NotImplementedException();
    }

    public void LogData(SurfaceAudioPlayer.DataLogged cube)
    {
        dataLogger.LogData(cube);
        cubeMover.selectedCube.Mute();
        cubeMover.selectedCube.selected = false;
    }

    public void LogData(string data)
    {
        throw new NotImplementedException();
    }

    public void LogError(string data)
    {
        throw new NotImplementedException();
    }

    public void GetRandomNewPath()
    {
        dataLogger.GetRandomNewPath();
        cubeMover.selectedCube.Mute();
        cubeMover.selectedCube.selected = false;
    }
}
