using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayingMode : MonoBehaviour
{
    public void ModeSetting(bool on)
    {
        GameManager.instance.SetPlayingMode(on);
        GameManager.instance.currentGameScore = new float[4];
    }
}
