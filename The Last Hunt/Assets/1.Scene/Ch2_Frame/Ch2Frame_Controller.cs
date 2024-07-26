using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch2Frame_Controller : MonoBehaviour
{
    private double checkTime;
    private double current_time = 0;
    private readonly int BPM = 130;
    private int count = 0;
    [SerializeField] private GameObject sampleAnimal, HitSmoke;
    [SerializeField] private Animator Hunter_ani;

    private void Start()
    {
        checkTime = AudioSettings.dspTime;
        Hunter_ani.SetTrigger("StartBGM");
    }

    private void Update()
    {
        current_time += AudioSettings.dspTime - checkTime;
        checkTime = AudioSettings.dspTime;

        if (current_time > 60d / BPM)
        {
            count++;
            current_time -= (60d / BPM);
            switch(count % 4)
            {
                case 1:
                    sampleAnimal.SetActive(true);
                    break;
                case 3:
                    Hunter_ani.SetTrigger("Swing");
                    sampleAnimal.SetActive(false);
                    HitSmoke.GetComponent<ParticleSystem>().Play();
                    break;
            }
        }
    }

}
