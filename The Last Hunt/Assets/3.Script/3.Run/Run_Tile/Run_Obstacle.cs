using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_Obstacle : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {


        //if(isGameover)return;


        if(other.CompareTag("Player"))
        {
            //player Á×À½
        }
    }
}
