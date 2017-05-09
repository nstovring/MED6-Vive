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
    public static string[] fileNames = { "Visual", "VibroTactile", "Vibro-Visual" };
    public static string[] pathNames = { "Straight", "Curved", "Very Curved" };

    static int testCount = 0;

    public PathIterator pathIterator;

    public static DataLogger Instance;

    float[] pathWidthTypes = { 0.25f, 0.50f, 1f };
    float[] pathLengthTypes = { 0.25f, 0.50f, 1f };
    int[] pathShapeTypes = { 0, 1, 2};
    int[] testTypes = { 0, 1, 2};
    public int testAmount = 0;


    List<TaskVariables> taskVariablesVisual;
    List<TaskVariables> taskVariablesTactile;
    List<TaskVariables> taskVariablesBoth;
    // Use this for initialization
    void Start () {
       
	}

    public void Initialize()
    {

        Instance = this;

        testAmount = testTypes.Length * pathShapeTypes.Length * pathLengthTypes.Length * pathWidthTypes.Length;

        taskVariablesVisual = new List<TaskVariables>();
        taskVariablesTactile = new List<TaskVariables>();
        taskVariablesBoth = new List<TaskVariables>();


        for (int i = 0; i < testTypes.Length; i++)
        {
            for (int j = 0; j < pathShapeTypes.Length; j++)
            {
                for (int k = 0; k < pathLengthTypes.Length; k++)
                {
                    for (int l  = 0; l < pathWidthTypes.Length; l++)
                    {
                        TaskVariables tmp;
                        tmp.pathLength = (pathLengthTypes[k]);
                        tmp.pathWidth = (pathWidthTypes[l ]);
                        tmp.pathType = (pathShapeTypes[j]);
                        tmp.testCondition = (testTypes[i]);

                        switch (tmp.testCondition)
                        {
                            case 0:
                                taskVariablesVisual.Add(tmp);
                                break;
                            case 1:
                                taskVariablesTactile.Add(tmp);
                                break;
                            case 2:
                                taskVariablesBoth.Add(tmp);
                                break;
                        }
                    }
                }
            }
        }


        //for (int i = 0; i < testAmount; i++)
        //{
        //    TaskVariables tmp;
        //    tmp.pathLength = (pathLengthTypes[i % pathLengthTypes.Length]);
        //    tmp.pathWidth = (pathWidthTypes[i % pathWidthTypes.Length]);
        //    tmp.pathType = (pathShapeTypes[i % pathShapeTypes.Length]);
        //    tmp.testCondition = (testTypes[i % testTypes.Length]);

        //    switch (tmp.testCondition)
        //    {
        //        case 0:
        //            taskVariablesVisual.Add(tmp);
        //            break;
        //        case 1:
        //            taskVariablesTactile.Add(tmp);
        //            break;
        //        case 2:
        //            taskVariablesBoth.Add(tmp);
        //            break;
        //    }

        //}

        Debug.Log(taskVariablesVisual.Count);
        Debug.Log(taskVariablesTactile.Count);
        Debug.Log(taskVariablesBoth.Count);

        RandomizeTest();
        //AddOrderedTests();
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
    public List<TaskVariables> taskVariables;
    public struct TaskVariables
    {
        public int testCondition;
        public int pathType;
        public float pathWidth;
        public float pathLength;
    }

    public void RandomizeTest()
    {
        taskVariables = new List<TaskVariables>();
        List<List<TaskVariables>> listOfLists = new List<List<TaskVariables>>();
        taskVariablesVisual = RandomizeList(taskVariablesVisual);
        taskVariablesTactile = RandomizeList(taskVariablesTactile);
        taskVariablesBoth = RandomizeList(taskVariablesBoth);
        listOfLists.Add(taskVariablesVisual);
        listOfLists.Add(taskVariablesTactile);
        listOfLists.Add(taskVariablesBoth);

        for (int i = 0; i < listOfLists.Count; i++)
        {
            int rng = UnityEngine.Random.Range(0, listOfLists.Count - 1);

            List<TaskVariables> testTypetemp = listOfLists[rng];

            listOfLists[rng] = listOfLists[i];
            listOfLists[i] = testTypetemp;
        }

        for (int i = 0; i < listOfLists.Count; i++)
        {
            taskVariables.AddRange(listOfLists[i]);
        }
    }

    List<TaskVariables> RandomizeList(List<TaskVariables> input)
    {
        for (int i = 0; i < input.Count; i++)
        {
            int rng = UnityEngine.Random.Range(0, input.Count - 1);

            TaskVariables testTypetemp = input[rng];

            input[rng] = input[i];
            input[i] = testTypetemp;
        }
        return input;
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
        if (testProgression > testAmount-1)
        {
            Debug.Log("Test Complete");
            ResetTest();
        }
        pathIterator.IteratePath(taskVariables[testProgression]);
       
    }

    public void ResetTest()
    {
        testProgression = 0;
    }
}
