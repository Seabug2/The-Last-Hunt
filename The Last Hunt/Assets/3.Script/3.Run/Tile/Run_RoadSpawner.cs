using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_RoadSpawner : MonoBehaviour
{

    [SerializeField] List<Run_Road> roads;
    
    Run_Road lastRoad;
    private int r_index;
    private Transform tempPos;


    private void Start()
    {
        List<Run_Road> roads = new List<Run_Road>();
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
        lastRoad = road;
        road.gameObject.SetActive(true);
        if(roads == null)
        {
            roads.Add(road);
        }
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


    public void ReturnList(Transform parentTransform,string layerName)
    {
        Debug.Log("returnList에 들어왔습니다.");
        int layer = LayerMask.NameToLayer(layerName);

        if(layer ==-1)
        {
            Debug.Log($"Layer '{layerName}'이 없습니다.");
        }
        //여기서는 리스트로 다시 돌아가는 계산만 해줄꺼
        
            Debug.Log("아 오브젝트 비활성화 준비");
        this.gameObject.SetActive(false);
            Debug.Log("아 오브젝트 비활성화 완료");

        //여기서 비활성화한 오브젝트의 자식객체의 Layer를 탐색하여 다시 Tile로 바꿔줌
        foreach (Transform childTransform in parentTransform)
        {
            if (childTransform.name == "Water")
            {
                childTransform.gameObject.layer = 6;
                
            }

        }
        
    }
}
