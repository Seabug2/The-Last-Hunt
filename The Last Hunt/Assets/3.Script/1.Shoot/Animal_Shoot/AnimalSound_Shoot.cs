using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSound_Shoot : MonoBehaviour
{
    [SerializeField] private AudioSource audio_s;
    [SerializeField] private AudioClip animalSound;
    [SerializeField] private AudioClip walking;
    [SerializeField] private AudioClip death;

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
        audio_s.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
    }
}
