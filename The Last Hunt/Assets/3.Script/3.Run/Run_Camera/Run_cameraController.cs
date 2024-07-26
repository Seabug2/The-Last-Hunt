using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Run_cameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera ingameVirCam;

    private void Awake()
    {
        ingameVirCam.Priority = 0;
       

    }
    public void ShiftVirtualCam()
    {
        StartCoroutine("WaitTime");
    }
    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(3.5f);
        ingameVirCam.Priority = 15;
    }
}
