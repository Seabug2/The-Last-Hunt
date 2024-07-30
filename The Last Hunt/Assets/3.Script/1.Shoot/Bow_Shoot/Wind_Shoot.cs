using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind_Shoot : MonoBehaviour
{
    public static Vector3 windDir;
    public static float windStr;
    private float randTime;
    private float startTime;
    private float endTime;

    [SerializeField] private float minTerm = 20f;
    [SerializeField] private float maxTerm = 40f;
    [SerializeField] private float minAcc = 0f;
    [SerializeField] private float maxAcc = 0.05f;

    private void Awake()
    {
        randTime = Random.Range(minTerm, maxTerm);
        startTime = Time.time;
        SetWind();
        Debug.Log($"Interval : {randTime}");
        Debug.Log($"Direction : {windDir}");
        Debug.Log($"Strength : {windStr}");
    }

    private void Update()
    {
        endTime = Time.time;
        if (endTime - startTime >= randTime)
        {
            SetWind();
            randTime = Random.Range(minTerm, maxTerm);
            startTime = Time.time;
            //Debug.Log($"Interval : {randTime}");
            //Debug.Log($"Direction : {windDir}");
            //Debug.Log($"Strength : {windStr}");
        }
        else
        {
            return;
        }
    }

    private void SetWind()
    {
        windDir = new Vector3(Random.Range(-180, 180), 0, Random.Range(-180, 180));
        windStr = Random.Range(minAcc, maxAcc);
    }
}
