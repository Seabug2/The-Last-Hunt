using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{

    [SerializeField] public GameObject[] roadPreFabs;
    public Queue<GameObject> road_q = new Queue<GameObject>();
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform nextPos;
    private int r_index;
    private Transform tempPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            //오브젝트 생성
            InstantiateRoad();
            this.gameObject.transform.position = nextPos.position;
            this.gameObject.transform.forward = nextPos.forward;
            this.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        

        Queue<GameObject> road_q = new Queue<GameObject>(roadPreFabs);
        for(int i = 0; i<roadPreFabs.Length;i++)
        {
            road_q.Enqueue(roadPreFabs[i]);
            if (road_q == null)
            {
                road_q.Enqueue(roadPreFabs[i]);
            }
            
        }
        


    }

    private void InstantiateRoad()
    {
        if(road_q.Count>0)
        {
            GameObject road = road_q.Dequeue();
            r_index = Random.Range(0, road_q.Count);
            Instantiate(roadPreFabs[r_index], tempPos.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("응 q안에 아무것도 없어");
        }
    }
}
