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
    public GameObject obstacleRoad;//길이 점프를 해야하는 길이면 이 오브젝트를 끄겠다
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
            spawner.InstantiateRoad(nextRoadPosition.position, nextRoadPosition.forward);
            
        }
        else if (other.CompareTag("Animal"))
        {
            GetComponent<Animator>().enabled = true;
            Debug.Log($"{transform.name}");
            spawner.ReturnToPool(this);
            Debug.Log("레이어 원상복구");
        }
    }

    public void OnEnableObstacle()
    {
        int randomValue = Random.Range(0, 2);
        int randomIndex = Random.Range(0, myObstacle.Length);
        /*
        if (randomValue == 0)
        {
            obstacleRoad.SetActive(false);
            
            Debug.Log("value는 0입니다.");
            return;
            //for jump
        }
        else if(randomValue != 0)
        {

            obstacleRoad.SetActive(true);

            Debug.Log("value는 1입니다.");
            // 모든 장애물을 비활성화
            for (int i = 0; i < myObstacle.Length; i++)
            {
                
                if (i == randomIndex) 
                myObstacle[i].SetActive(true);
               else
                myObstacle[i].SetActive(false);
                    
            }
            return;
        }
        */
        obstacleRoad.SetActive(true);

        Debug.Log("value는 1입니다.");
        // 모든 장애물을 비활성화
        for (int i = 0; i < myObstacle.Length; i++)
        {

            if (i == randomIndex)
                myObstacle[i].SetActive(true);
            else
                myObstacle[i].SetActive(false);

        }
    }


}
