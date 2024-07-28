using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController_Shoot : MonoBehaviour
{
    [SerializeField] private float timeMultiplier;
    [SerializeField] private float startHour;
    [SerializeField] private Text timeText;
    [SerializeField] private Light sunLight;
    [SerializeField] private float sunriseHour;
    [SerializeField] private float sunsetHour;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private UIController_Shoot ui;

    private DateTime currentTime;
    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;

    private void Awake()
    {
        Time.timeScale = 1;
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }
    
    private void Update()
    {
        if (currentTime.TimeOfDay > sunsetTime)
        {
            return;
        }
        if (currentTime.TimeOfDay <= sunsetTime)
        {
            UpdateTimeOfDay();
            RotateSun();
            if (currentTime.TimeOfDay > sunsetTime)
            {
                ui.ResultScreen();
                enabled = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            ui.TotalKill_Score = 3000;
            ui.ResultScreen();
        }
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    private TimeSpan CalcDeltaTime(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan delta = toTime - fromTime;

        if (delta.TotalSeconds < 0)
        {
            delta += TimeSpan.FromHours(24);
        }

        return delta;
    }

    private void RotateSun()
    {
        float sunLightRotation = 0f;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan dayDuration = CalcDeltaTime(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalcDeltaTime(sunriseTime, currentTime.TimeOfDay);

            double dayProgression = timeSinceSunrise.TotalMinutes / dayDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)dayProgression);

            timeSlider.maxValue = (float)dayDuration.TotalMinutes;
            timeSlider.value = (float)timeSinceSunrise.TotalMinutes;
        }
        else
        {
            TimeSpan nightDuration = CalcDeltaTime(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalcDeltaTime(sunsetTime, currentTime.TimeOfDay);

            double nightProgression = timeSinceSunset.TotalMinutes / nightDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)nightProgression);
        }
        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }
}
