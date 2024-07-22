using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSound_Shoot : MonoBehaviour
{
    [SerializeField] private AudioClip animalSound;
    [SerializeField] private AudioClip walking;
    [SerializeField] private AudioClip running;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioSource audio_s;

    [SerializeField] private AnimalController_Shoot animal;

    private void Awake()
    {
        TryGetComponent(out audio_s);
        TryGetComponent(out animal);
    }

    private void Update()
    {
        switch (animal.CurrentState)
        {
            case AnimalController_Shoot.WanderState.Idle:
                if (animalSound != null)
                {
                    StartCoroutine(PlaySound_co(animalSound));
                }
                break;
            case AnimalController_Shoot.WanderState.Wander:
                if (walking != null)
                {
                    StartCoroutine(PlaySound_co(walking));
                }
                break;
            case AnimalController_Shoot.WanderState.Chase:
                if (running != null)
                {
                    StartCoroutine(PlaySound_co(running));
                }
                break;
            case AnimalController_Shoot.WanderState.Evade:
                if (running != null)
                {
                    StartCoroutine(PlaySound_co(running));
                }
                break;
            case AnimalController_Shoot.WanderState.Dead:
                if (death != null)
                {
                    StartCoroutine(PlaySound_co(death));
                }
                break;
            default:
                break;
        }
    }
    private IEnumerator PlaySound_co(AudioClip clip)
    {
        audio_s.Stop();
        audio_s.clip = clip;
        audio_s.Play();
        yield return new WaitForSeconds(clip.length);
    }

    public void PlayDeathSound()
    {
        if (death != null)
        {
            StartCoroutine(PlaySound_co(death));
        }
    }
}
