using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm_AnimalController : MonoBehaviour
{
    public Vector3 startMarker;   // 시작 위치
    public Vector3 endMarker;     // 끝 위치

    private float startTime;
    private float reachTime;
    float fracJourney = 0;

    private void OnEnable()
    {
        startMarker = Rhythm_AnimalPooling.instance.Spawner.position;
        endMarker = Rhythm_AnimalPooling.instance.targetPoint.position;
        Vector3 length = endMarker - startMarker;
        endMarker = startMarker + length * 1.25f;

        startTime = Time.time;
        reachTime = 60f / 130 * 2 * 1.25f;
    }

    private void Update()
    {
        float distCovered = Time.time - startTime;
        fracJourney = distCovered / reachTime;
        transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);
        if (fracJourney > 0.99f)
        {
            Rhythm_SoundManager.instance.PlaySFX("Miss");
            Rhythm_ChapterManager.instance.CountAdd(0);
            Rhythm_AnimalPooling.instance.ReturnObjectToPool(gameObject);
        }
    }

}
