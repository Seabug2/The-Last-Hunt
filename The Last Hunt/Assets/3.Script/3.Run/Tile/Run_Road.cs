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
    //���� Ÿ���� ��� ��Ȱ��ȭ �� �� �ִ� Ÿ�ϵ�?


    //���� �÷��̾ ��� �ִ� �� ������Ʈ
    private void OnTriggerEnter(Collider other)
    {



        if (other.CompareTag("Player"))
        {
            
        }
    }



}
