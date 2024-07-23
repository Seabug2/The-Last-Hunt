using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_RoadSpawner : MonoBehaviour
{

    [SerializeField] List<Run_Road> roads;
    public Queue<GameObject> road_q = new Queue<GameObject>();
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform nextPos;
    private int r_index;
    private Transform tempPos;


    private void Start()
    {
    }

    /// <summary>
    /// 지정한 위치에, 지정한 방향으로 랜덤한 길을 현재 플레이어가 밟고 있는 길 오브젝트 앞에 생성합니다.
    /// </summary>
    public  void InstantiateRoad(Vector3 spawnPosition, Vector3 spawnDir)
    {
        Run_Road road = roads[Random.Range(0, roads.Count)];
        roads.Remove(road);
        road.transform.position = spawnPosition;
        road.transform.forward = spawnDir;
        //if(road_q.Count==0)
        //{
        //    r_index = Random.Range(0, road_q.Count);
        //    GameObject road  = Instantiate(roadPreFabs[r_index], tempPos.position, Quaternion.identity);
        //    this.gameObject.SetActive(false);
        //    return road;
        //}
        //else
        //{
        //    GameObject road = road_q.Dequeue();
        //    return road;
        //}
    }
}
