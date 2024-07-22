using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm_Pause : MonoBehaviour
{
    private bool isPause = false;

    public void RhythmPause()
    {
        isPause = !isPause;
        Rhythm_ChapterManager.instance.BGMisPausing = isPause;
        if (isPause)
        {
            Rhythm_SoundManager.instance.BGMPlayer.Pause();
        }
        else
        {
            Rhythm_SoundManager.instance.BGMPlayer.UnPause();
        }
    }
}
