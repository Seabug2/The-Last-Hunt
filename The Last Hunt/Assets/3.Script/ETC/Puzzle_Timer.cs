using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle_Timer : MonoBehaviour
{
    [SerializeField]
    Text timeText;

    void Start()
    {
        timeText.gameObject.SetActive(false);
    }

    IEnumerator CountTime()
    {
        float startTime = Time.time;
        timeText.gameObject.SetActive(true);

        float elapsed = 0;

        while (elapsed < 10)
        {
            yield return null;
            elapsed = Time.time - startTime;
            timeText.text = "Time\n" + TimerConvert(elapsed);
        }

        Destroy(timeText.gameObject);
        Destroy(this);
    }

    public static string TimerConvert(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int milliseconds = (int)(time * 100) % 100;

        return $"{minutes:00}:{seconds:00}<size=25>.{milliseconds:00}</size>";
    }
}
