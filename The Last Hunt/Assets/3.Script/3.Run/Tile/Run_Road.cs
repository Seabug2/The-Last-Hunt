using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_Road : MonoBehaviour
{
    [SerializeField]
    Vector3 nextRoadPosition;

    [SerializeField]
    GameObject[] myObstacle;

    Run_RoadSpawner spawner;
    private void Awake()
    {
        
    }
    //점프 타일일 경우 비활성화 할 수 있는 타일들?


    //현재 플레이어가 밟고 있는 길 오브젝트
    private void OnTriggerEnter(Collider other)
    {



        if (other.CompareTag("Player"))
        {
            
        }
    }



}
