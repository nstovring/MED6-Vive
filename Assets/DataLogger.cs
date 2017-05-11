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

    public SurfaceAudioPlayer cube;
    public List<SurfaceAudioPlayer> cubes;
    static string fileName = "MyFile.txt";
    public static string[] fileNames = { "Visual", "Vibro-Visual_Right", "Vibro-Visual_Left" };
    public static string[] pathNames = { "Straight", "Curved", "Very Curved" };

    static int testCount = 0;

    public PathIterator pathIterator;

    public static DataLogger Instance;

    float[] pathWidthTypes = { 0.25f, 0.66f, 1f };
    float[] pathLengthTypes = { 0.30f, 0.60f, 1f };
    int[] pathShapeTypes = { 0, 1, 2};
    int[] testTypes = { 0, 1, 2};
    public int testAmount = 0;


    List<TaskVariables> taskVariablesVisual;
    List<TaskVariables> taskVariablesBoth_Right;
    List<TaskVariables> taskVariablesBoth_Left;
    // Use this for initialization
    void Start () {
       
	}

    public void Initialize()
    {
        Instance = this;

        testAmount = testTypes.Length * pathShapeTypes.Length * pathLengthTypes.Length * pathWidthTypes.Length;

        taskVariablesVisual = new List<TaskVariables>();
        taskVariablesBoth_Right = new List<TaskVariables>();
        taskVariablesBoth_Left = new List<TaskVariables>();
        loggedData = new List<SurfaceAudioPlayer.DataLogged>();
        loggedDataVisual = new List<SurfaceAudioPlayer.DataLogged>();
        loggedDataVibro_Tactile_Right = new List<SurfaceAudioPlayer.DataLogged>();
        loggedDataVibro_Tactile_Left = new List<SurfaceAudioPlayer.DataLogged>();

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
                        tmp.pathWidth = (pathWidthTypes[l]);
                        tmp.pathType = (pathShapeTypes[j]);
                        tmp.testCondition = (testTypes[i]);
                        tmp.logThisData = true;

                        switch (tmp.testCondition)
                        {
                            case 0:
                                taskVariablesVisual.Add(tmp);
                                break;
                            case 1:
                                taskVariablesBoth_Right.Add(tmp);
                                break;
                            case 2:
                                taskVariablesBoth_Left.Add(tmp);
                                break;
                        }
                    }
                }
            }
        }
        taskVariables = new List<TaskVariables>();
        AddOrderedTests();
        RandomizeTest();
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
        public bool logThisData;
    }

    public void RandomizeTest()
    {
      
        List<List<TaskVariables>> listOfLists = new List<List<TaskVariables>>();
        taskVariablesVisual = RandomizeList(taskVariablesVisual);
        taskVariablesBoth_Right = RandomizeList(taskVariablesBoth_Right);
        taskVariablesBoth_Left = RandomizeList(taskVariablesBoth_Left);
        listOfLists.Add(taskVariablesVisual);
        listOfLists.Add(taskVariablesBoth_Right);
        listOfLists.Add(taskVariablesBoth_Left);

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

    void AddOrderedTests()
    {
        float[] pathWidthTypesPre = { 0.25f, 0.66f, 1f};
        float[] pathLengthTypesPre = {1f};
        int[] pathShapeTypesPre = {0, 1, 2};
        int[] testTypesPre = { 0, 1, 2 };


        for (int i = 0; i < testTypesPre.Length; i++)
        {
            for (int j = 0; j < pathShapeTypesPre.Length; j++)
            {
                for (int k = 0; k < pathLengthTypesPre.Length; k++)
                {
                    for (int l = 0; l < pathWidthTypesPre.Length; l++)
                    {
                        TaskVariables tmp;
                        tmp.pathLength = (pathLengthTypesPre[k]);
                        tmp.pathWidth = (pathWidthTypesPre[l]);
                        tmp.pathType = (pathShapeTypesPre[j]);
                        tmp.testCondition = (testTypesPre[i]);
                        tmp.logThisData = false;
                        taskVariables.Add(tmp);
                    }
                }
            }
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

    void LogDataPath(string data)
    {

        var sr = File.AppendText(fileNames[taskVariables[testProgression].testCondition] + "_Results.txt");
        sr.WriteLine(data);

        sr.Close();
    }

    List<SurfaceAudioPlayer.DataLogged> loggedData;
    List<SurfaceAudioPlayer.DataLogged> loggedDataVisual;
    List<SurfaceAudioPlayer.DataLogged> loggedDataVibro_Tactile_Right;
    List<SurfaceAudioPlayer.DataLogged> loggedDataVibro_Tactile_Left;

    void LogDataPath(SurfaceAudioPlayer.DataLogged data)
    {
        loggedData.Add(data);
    }

    void SortData()
    {
        for (int i = 0; i < loggedData.Count; i++)
        {
            switch (loggedData[i].testCondiction)
            {
                case 0:
                    loggedDataVisual.Add(loggedData[i]);
                    break;
                case 1:
                    loggedDataVibro_Tactile_Right.Add(loggedData[i]);
                    break;
                case 2:
                    loggedDataVibro_Tactile_Left.Add(loggedData[i]);
                    break;
            }
        }

        loggedDataVisual = loggedDataVisual.OrderBy(x => x.pathType).ToList();
        loggedDataVibro_Tactile_Right = loggedDataVibro_Tactile_Right.OrderBy(x => x.pathType).ToList();
        loggedDataVibro_Tactile_Left = loggedDataVibro_Tactile_Left.OrderBy(x => x.pathType).ToList();

        List<List<SurfaceAudioPlayer.DataLogged>> ListInList = new List<List<SurfaceAudioPlayer.DataLogged>>();

        ListInList.Add(loggedDataVisual);
        ListInList.Add(loggedDataVibro_Tactile_Right);
        ListInList.Add(loggedDataVibro_Tactile_Left);

        for (int i = 0; i < ListInList.Count; i++)
        {
            List<List<SurfaceAudioPlayer.DataLogged>> ListInListtemp = new List<List<SurfaceAudioPlayer.DataLogged>>();
            List<SurfaceAudioPlayer.DataLogged> loggedDataTemp1 = new List<SurfaceAudioPlayer.DataLogged>();
            List<SurfaceAudioPlayer.DataLogged> loggedDataTemp2 = new List<SurfaceAudioPlayer.DataLogged>();
            List<SurfaceAudioPlayer.DataLogged> loggedDataTemp3 = new List<SurfaceAudioPlayer.DataLogged>();

            ListInListtemp.Add(loggedDataTemp1);
            ListInListtemp.Add(loggedDataTemp2);
            ListInListtemp.Add(loggedDataTemp3);

            //Foreach list in list split into three seperate lists and order by ID
            List<SurfaceAudioPlayer.DataLogged> tempData = ListInList[i];
            int arrayIncrement = 0;
                
            for (int k = 0; k < ListInListtemp.Count; k++)
            {
                for (int j = 0; j < tempData.Count/3; j++)
                {
                    ListInListtemp[k].Add(tempData[arrayIncrement]);
                    arrayIncrement++;
                }
            }

            ListInListtemp[0].CopyTo(loggedDataTemp1.ToArray());
            ListInListtemp[1].CopyTo(loggedDataTemp2.ToArray());
            ListInListtemp[2].CopyTo(loggedDataTemp3.ToArray());

            for (int j  = 0; j < loggedDataTemp1.Count; j++)
            {
            }

            loggedDataTemp1 = loggedDataTemp1.OrderBy(x => x.ID).ToList();
            loggedDataTemp2 = loggedDataTemp2.OrderBy(x => x.ID).ToList();
            loggedDataTemp3 = loggedDataTemp3.OrderBy(x => x.ID).ToList();


            ListInList[i] = new List<SurfaceAudioPlayer.DataLogged>();
            ListInList[i].AddRange(loggedDataTemp1);
            ListInList[i].AddRange(loggedDataTemp2);
            ListInList[i].AddRange(loggedDataTemp3);
        }
        loggedData = new List<SurfaceAudioPlayer.DataLogged>();

        for (int i = 0; i < ListInList.Count; i++)
        {
            loggedData.AddRange(ListInList[i]);
        }
    }

    [MenuItem("Test Menu/Add Dummy Data")]
    static void DummyData()
    {
        List<TaskVariables> tempList =  DataLogger.Instance.taskVariables;
        for (int i = tempList.Count/4; i < tempList.Count; i++)
        {
            TaskVariables task = tempList[i];
            SurfaceAudioPlayer.DataLogged dum = new SurfaceAudioPlayer.DataLogged();
            dum.accuracy = -1;
            dum.pathType = task.pathType;
            dum.ID = UnityEngine.Random.Range(1f, 6f);
            dum.elapsedTime = UnityEngine.Random.Range(0f, 11f);
            dum.testCondiction = task.testCondition;
            DataLogger.Instance.loggedDataVisual.Add(dum);
        }
    }
    void WriteDataToFile()
    {
        for (int i = 0; i < loggedData.Count; i++)
        {
            switch (loggedData[i].testCondiction)
            {
                case 0:
                    var sr = File.AppendText(fileNames[loggedData[i].testCondiction] + "_Results.txt");
                    sr.WriteLine(loggedData[i].GetData());
                    sr.Close();
                    break;
                case 1:
                    var se = File.AppendText(fileNames[loggedData[i].testCondiction] + "_Results.txt");
                    se.WriteLine(loggedData[i].GetData());
                    se.Close();
                    break;
                case 2:
                    var sw = File.AppendText(fileNames[loggedData[i].testCondiction] + "_Results.txt");
                    sw.WriteLine(loggedData[i].GetData());
                    sw.Close();
                    break;
            }
        }
    }

    void ICustomMessageTarget.LogData(string data)
    {
        LogDataPath(data);
        
    }

    void ICustomMessageTarget.LogError(string data)
    {
        StreamWriter sr;
        if (!File.Exists("Error_Results.txt"))
        {
            sr = File.CreateText("Error_Results.txt");
            sr.Close(); 
        }
        sr = File.AppendText("Error_Results.txt");
        sr.WriteLine(data);

        sr.Close();
        GetRandomNewPath();
    }

    [HideInInspector]
    public int testProgression = 0;
    public void GetRandomNewPath()
    {
        testProgression++;
        if (testProgression > taskVariables.Count-1)
        {
            Debug.Log("Test Complete");
            ResetTest();
        }
        pathIterator.IteratePath(taskVariables[testProgression]);
       
    }

    public void ResetTest()
    {
        testProgression = 0;
        EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void LogData(SurfaceAudioPlayer cube, int testType)
    {
        throw new NotImplementedException();
    }

    public void LogData(SurfaceAudioPlayer.DataLogged cube)
    {
        LogDataPath(cube);
        GetRandomNewPath();
    }

    void OnDestroy()
    {
        SortData();
        WriteDataToFile();
    }
}
