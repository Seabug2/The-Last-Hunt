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
    //���� Ÿ���� ��� ��Ȱ��ȭ �� �� �ִ� Ÿ�ϵ�?


    //���� �÷��̾ ��� �ִ� �� ������Ʈ
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("dddd");
            spawner.InstantiateRoad(nextRoadPosition.position, nextRoadPosition.forward);
        }
    }

  


}
