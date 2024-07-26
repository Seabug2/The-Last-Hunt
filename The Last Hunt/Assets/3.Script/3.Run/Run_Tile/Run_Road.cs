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
    public GameObject obstacleRoad;//���� ������ �ؾ��ϴ� ���̸� �� ������Ʈ�� ���ڴ�
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
            spawner.InstantiateRoad(nextRoadPosition.position, nextRoadPosition.forward);
            
        }
        else if (other.CompareTag("Animal"))
        {
            GetComponent<Animator>().enabled = true;
            Debug.Log($"{transform.name}");
            spawner.ReturnToPool(this);
            Debug.Log("���̾� ���󺹱�");
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
            
            Debug.Log("value�� 0�Դϴ�.");
            return;
            //for jump
        }
        else if(randomValue != 0)
        {

            obstacleRoad.SetActive(true);

            Debug.Log("value�� 1�Դϴ�.");
            // ��� ��ֹ��� ��Ȱ��ȭ
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

        Debug.Log("value�� 1�Դϴ�.");
        // ��� ��ֹ��� ��Ȱ��ȭ
        for (int i = 0; i < myObstacle.Length; i++)
        {

            if (i == randomIndex)
                myObstacle[i].SetActive(true);
            else
                myObstacle[i].SetActive(false);

        }
    }


}
