using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rhythm_AnimalSpawner : MonoBehaviour
{
    // public Rhythm_AnimalPooling animalPool;
    private GameObject obj;
    private double current_time = -0.07f;
    private int BPM = 130;
    private int count = 1;
    private int[] animal_appear;

    private void Start()
    {
        animal_appear = new int[127]
        {
            0,0,0,0,0,0,0,0,0,0,
            0,0,1,0,0,0,0,0,0,0,
            1,0,0,0,0,0,0,0,1,0,
            0,0,1,0,0,0,1,0,0,1,
            1,0,0,0,1,0,0,0,0,0,
            0,0,1,0,0,0,0,0,0,0,

            1,0,0,0,1,0,0,0,1,0,
            0,0,1,0,0,0,1,0,0,0,
            1,0,0,0,1,0,0,1,1,0,
            0,0,1,0,0,0,1,0,0,0,
            1,0,0,0,1,1,0,0,1,0,

            0,0,1,1,0,0,1,0,0,0,
            1,1,0,0,0,0,0
        };
    }

    private void Update()
    {
        if (count > 126) return;
        if (!Rhythm_SoundManager.instance.BGMisPlaying()) return;
        current_time += Time.deltaTime;
        if (current_time > 60d / BPM)
        {
            if (animal_appear[count] < 1)
            {
                // 안 나오는 박자
            }
            else
            {
                Rhythm_AnimalPooling.instance.GetObjectFromPool();
            }
            count++;
            if (count > 126)
            {
                Rhythm_ChapterManager.instance.ResultAppear();
            }
            current_time -= (60d / BPM);
        }
    }
}