using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_Road : MonoBehaviour
{
    [SerializeField]
    Transform nextRoadPosition;

    [SerializeField]
    GameObject[] myObstacle;
    Run_RoadSpawner spawner;

    private void Awake()
    {
        spawner = FindObjectOfType<Run_RoadSpawner>();

        Transform parentTranform = this.transform;
        if (nextRoadPosition == null)
        {
            foreach (Transform childTransform in parentTranform)
            {
                if (childTransform.name == "Next Tile Position")
                {
                    nextRoadPosition = childTransform;
                    break;
                }

            }
        }
    }
    //점프 타일일 경우 비활성화 할 수 있는 타일들?


    //현재 플레이어가 밟고 있는 길 오브젝트
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("dddd");
            spawner.InstantiateRoad(nextRoadPosition.position, nextRoadPosition.forward);
        }
    }

  


}
