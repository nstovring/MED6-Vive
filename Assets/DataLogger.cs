using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataLogger : MonoBehaviour {

    public List<SurfaceAudioPlayer> cubes;

    string fileName = "MyFile.txt";

    // Use this for initialization
    void Start () {
		
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

        if (File.Exists(fileName))
        {
            Debug.Log(fileName + " already exists.");
            return;
        }
        var sr = File.CreateText(fileName);
        sr.WriteLine("Threshold & User Selected Pos: ");

        foreach (var item in cubes)
        {
            sr.WriteLine(item.GetAccuracy());
        }
        
        sr.Close();


       
    }
}
