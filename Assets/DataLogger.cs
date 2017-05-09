using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using System;
using System.IO;
using System.Linq;

public class DataLogger : MonoBehaviour, ICustomMessageTarget
{

    public List<SurfaceAudioPlayer> cubes;

    static string fileName = "MyFile.txt";
    public static string[] fileNames = { "Visual", "VibroTactile", "Both" };
    static int testCount = 0;

    public PathIterator pathIterator;

    public int[] testsTypesArr = { 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2 };
    public int[] testsPathsArr = { 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3 };
    public float[] testsPathsWidthsArr = { 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3 };
    public float[] testsPathsLengthsArr = { 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3 };

    public static DataLogger Instance;

    float[] pathWidthTypes = { 0.25f, 0.50f, 1f };
    float[] pathLengthTypes = { 0.25f, 0.50f, 1f };
    // Use this for initialization
    void Start () {
       
	}

    public void Initialize()
    {
        Instance = this;
        testsTypesArr =  new int[]{ 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2 };
        testsPathsArr = new int[]{ 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3 };
        RandomizeTest();
        AddOrderedTests();
        foreach (var item in fileNames)
        {
            if (File.Exists(item+"_Results.txt"))
            {
                Debug.Log(item + " already exists.");
                Debug.Log("Remember to Iterate test");
            }
            else
            {
                var sr = File.CreateText(item + "_Results.txt");
                sr.Close();
            }
        }
    }

    public void RandomizeTest()
    {
        for (int i = 0; i < testsPathsArr.Length; i++)
        {
            int rng = UnityEngine.Random.Range(0, testsPathsArr.Length - 1);
            int temp = testsTypesArr[rng];
            testsTypesArr[rng] = testsTypesArr[i];
            testsTypesArr[i] = temp;

            temp = testsPathsArr[rng];
            testsPathsArr[rng] = testsPathsArr[i];
            testsPathsArr[i] = temp;
        }
    }

    public void AddOrderedTests()
    {
        int[] orderedTestTypesArr = { 2, 1,0};
        int orderedTestPath = 0;

        List<int> testsPathsArrList = testsPathsArr.ToList();
        List<int> testsTypesArrList = testsTypesArr.ToList();

        for (int i = 0; i < 3; i++)
        {
            testsPathsArrList.Add(orderedTestPath);
            testsTypesArrList.Add(orderedTestTypesArr[i]);
        }

        testsTypesArr = testsTypesArrList.ToArray().Reverse().ToArray();
        testsPathsArr = testsPathsArrList.ToArray().Reverse().ToArray();

    }
    [MenuItem("Test Menu/Delete Test Files")]
    static void DeleteTestFiles()
    {
        foreach (var item in fileNames)
        {
            if(File.Exists(item + "_Results.txt"))
            File.Delete(item + "_Results.txt");

            Debug.Log("Deleting Test Files");
        }
    }

   
    static void IterateTest()
    {
        fileName = "Test Results_" + testCount + ".txt";
        if (File.Exists(fileName))
        {
            Debug.Log(fileName + " already exists.");
            Debug.Log("Remember to Iterate test");
            return;
        }
        var sr = File.CreateText(fileName);

        sr.WriteLine("User Selected Pos: ");

        testCount++;
        sr.Close();
        Debug.Log("Doing Something...");
    }
   

    string loggedData;
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            LogData();
        }
	}

    void LogData()
    {
      
        var sr = File.AppendText(fileName);

        foreach (var item in cubes)
        {
            sr.WriteLine(item.testProperties.GetAccuracy());
            item.RandomizePositionAndThreshold();
        }
        
        sr.Close();
    }

    void LogDataPath()
    {

        var sr = File.AppendText(fileName);

        foreach (var item in cubes)
        {
            sr.WriteLine(item.testProperties.GetLoggedData());
            item.RandomizePositionAndThreshold();
        }

        sr.Close();
    }
    void LogDataPath(SurfaceAudioPlayer cube, int testType)
    {

        var sr = File.AppendText(fileNames[testType] + "_Results.txt");
        sr.WriteLine(cube.testProperties.GetLoggedData());

        sr.Close();
    }
    void ICustomMessageTarget.LogData(SurfaceAudioPlayer cube, int testType)
    {
        LogDataPath(cube, testType);
        GetRandomNewPath();
    }
    [HideInInspector]
    public int testProgression = 0;
    public void GetRandomNewPath()
    {
        testProgression++;
        if (testProgression > testsPathsArr.Length-1)
        {
            Debug.Log("Test Complete");
            ResetTest();
        }
        pathIterator.SwitchTestType((PathIterator.TestType)testsTypesArr[testProgression]);
        pathIterator.IteratePath(testsPathsArr[testProgression]);
       
    }

    public void ResetTest()
    {

    }
}
