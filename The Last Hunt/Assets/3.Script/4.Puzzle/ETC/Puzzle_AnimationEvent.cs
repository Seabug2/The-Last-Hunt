using UnityEngine;

public class Puzzle_AnimationEvent : MonoBehaviour
{
    void AudioPlay()
    {
        if (TryGetComponent(out AudioSource _audio))
        {
            _audio.Play();
        }
    }
    void AudioPlayOneShot(AudioClip _clip)
    {
        if (TryGetComponent(out AudioSource _audio))
        {
            _audio.PlayOneShot(_clip);
        }
    }
}
