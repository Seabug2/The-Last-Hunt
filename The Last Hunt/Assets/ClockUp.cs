using UnityEngine;

public class ClockUp : MonoBehaviour
{
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad5))
        {
            audioSource .pitch = Mathf.Clamp(audioSource.pitch + Time.deltaTime * 10, 1f, 10f);
        }
        else if (Input.GetKeyUp(KeyCode.Keypad5))
        {
            audioSource.pitch = 1; 
        }
    }
}
