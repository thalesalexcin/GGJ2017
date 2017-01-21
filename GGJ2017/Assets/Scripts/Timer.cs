using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour {

    private Text timerUI;
    [HideInInspector]
    public float currentTime;
    private bool isRunning;

	// Use this for initialization
	void Start () {
        timerUI = GetComponent<Text>();
        Reset();
        StartTimer();
    }

    public void Reset()
    {
        currentTime = 0;
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    // Update is called once per frame
    void Update () {
        if (isRunning)
        {
            currentTime += Time.deltaTime;
        }
        timerUI.text = currentTime.ToString("F2");
	}
}
