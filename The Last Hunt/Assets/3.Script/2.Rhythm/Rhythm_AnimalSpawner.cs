using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rhythm_AnimalSpawner : MonoBehaviour
{
    // public Rhythm_AnimalPooling animalPool;
    private double checkTime;
    private double current_time = 0;
    private int BPM = 130;
    private int count = 5;
    private int[] animal_appear;

    private void Start()
    {
        animal_appear = new int[127]
        {
            0,0,0,0,0,0,0,0,0,0,
            0,0,1,0, 0,0,3,4,
            0,0,1,0, 0,3,4,0,
            0,0,1,0, 4,0,1,3,
            0,0,1,0 ,0,1,1,0,

            0,0,1,0, 0,3,0,2,
            0,0,1,0, 0,2,3,4,
            0,0,1,0, 0,0,1,3,
            0,0,1,0, 0,2,1,2,
            0,0,1,0, 3,0,1,4,
            0,2,1,0, 3,1,1,0,

            0,0,1,0, 0,2,1,0,
            0,0,1,0, 0,2,1,1,
            0,0,1,0, 2,3,1,1,
            0,4,1,0, 4,3,1,1,
            0,0,0,0,0
        };
        checkTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        if (count > 126 || !Rhythm_ChapterManager.instance.MainBGMisPlaying || Rhythm_ChapterManager.instance.isPausing)
        {
            checkTime = AudioSettings.dspTime;
            return;
        }

        current_time += (AudioSettings.dspTime - checkTime) * Rhythm_ChapterManager.instance.GameSpeed;
        checkTime = AudioSettings.dspTime;

        if (current_time > 60d / BPM)
        {
            if (animal_appear[count] > 0 && animal_appear[count] < Rhythm_ChapterManager.instance.Wave + 2)
            {
                Rhythm_AnimalPooling.instance.GetObjectFromPool();
            }
            count++;
            if (count > 126)
            {
                Invoke("NextWave", 1.0f);
            }
            current_time -= (60d / BPM);
        }
    }

    private void NextWave()
    {
        if (Rhythm_ChapterManager.instance.Wave < 3)
        {
            count = 5;
        }
        Rhythm_ChapterManager.instance.NextWave();
    }
}