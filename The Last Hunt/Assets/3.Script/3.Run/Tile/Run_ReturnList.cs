using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_ReturnList : MonoBehaviour
{
    Run_RoadSpawner roads;
    

    private void OnTriggerExit(Collider other)
    {

        if(other.CompareTag("Animal"))
        {
            Debug.Log($"{other.transform.name}");
            roads.ReturnList(other.transform, "Water", "Tile");
            Debug.Log("레이어 원상복구");
        }


    }
    
}
