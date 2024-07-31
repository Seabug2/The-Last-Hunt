using I18N.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_Skip : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Run_Manager.instance.IsGameOver) return;
            Run_Manager.instance.timer.time = Run_Manager.instance.TargetTime;
            Run_Manager.instance.EndEvent?.Invoke();
        }
    }
}
