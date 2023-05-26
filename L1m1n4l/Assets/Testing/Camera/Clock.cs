using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clock : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    [Header("Time Values")]
    [Range(0,60)]
    public int seconds;
    [Range(0,60)]
    public int minutes;
    [Range(0,60)]
    public int hours;

    public float currentSeconds;
    public int timerDefault;
    
    void Start() 
    {
        timerDefault = 0;
        timerDefault += (seconds + (minutes * 60) + (hours * 60 * 60));
        currentSeconds = timerDefault;  
    }

    void Update()
    {
        if((currentSeconds += Time.deltaTime) <= 0)
        {
        }
        else
        {
            timerText.text = TimeSpan.FromSeconds(currentSeconds).ToString(@"hh\:mm\:ss");
        }
    }

    
    
    
    
    
    
    
    
    
    /*public TMP_Text cameraText;

    void Awake() 
    {
        cameraText = GetComponent<TMP_Text>();    
    }

    private void Update() 
    {
        DateTime time = DateTime.Now;
        string hour = LeadingZero(time.Hour);
        string minute = LeadingZero(time.Hour);
        string second = LeadingZero(time.Second);

        cameraText.text = hour +":"+minute +":" +second;
    }

    string LeadingZero (int n)
    {
        return n.ToString().PadLeft(2, '0');
    }*/
}

