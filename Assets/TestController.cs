using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
//Class which controls when behaviour occurs during test
//Mainly controls when classes update
//Maybe UI Stuff
public class TestController : MonoBehaviour {
    public PathIterator pathIterator;
    public DataLogger dataLogger;
    public CubeMover cubeMover;
    public SurfaceAudioPlayer cube;

    public Text testTypeText;

    public bool tightThreshold = false;

	// Use this for initialization
	void Start () {
        dataLogger.Initialize();
        pathIterator.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
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
    }

    void UpdateUI()
    {
        testTypeText.text = " Test Num:"+ dataLogger.testProgression +", Test Type:"+ DataLogger.fileNames[(int)pathIterator.myTestType];
    }
}
