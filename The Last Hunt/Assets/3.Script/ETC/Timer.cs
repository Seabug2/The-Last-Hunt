using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Timer : MonoBehaviour
{
    public double time = 0;
    
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
        text.text = ConvertTimeCode(time);
    }

    public string ConvertTimeCode(double time)
    {
        // Get total seconds and then calculate minutes, seconds, and milliseconds
        int totalSeconds = (int)time;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        int milliseconds = (int)((time - totalSeconds) * 1000);

        // Format string as MM:SS:MS
        return string.Format("{0:D2}:{1:D2}.{2:D3}", minutes, seconds, milliseconds);
    }
}
