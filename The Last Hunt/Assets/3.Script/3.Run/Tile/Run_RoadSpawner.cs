using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_RoadSpawner : MonoBehaviour
{

    [SerializeField] List<Run_Road> roads = new List<Run_Road>();
    
    Run_Road lastRoad;

    private List<Run_Road> activeRoads = new List<Run_Road>();                 //추가
    private List<Run_Road> inactiveRoads = new List<Run_Road>();               //추가


    private void Start()
    {
        //List<Run_Road> roads = new List<Run_Road>();
        foreach (var road in roads)
        {
            road.gameObject.SetActive(false);
            inactiveRoads.Add(road);
        }
    }

    /// <summary>
    /// 지정한 위치에, 지정한 방향으로 랜덤한 길을 현재 플레이어가 밟고 있는 길 오브젝트 앞에 생성합니다.
    /// </summary>
    public  void InstantiateRoad(Vector3 spawnPosition, Vector3 spawnDir)
    {
        int randomIndex = Random.Range(0, inactiveRoads.Count);     //추가
        Run_Road road = inactiveRoads[randomIndex];                 //추가
        inactiveRoads.RemoveAt(randomIndex);                        //추가
        activeRoads.Add(road);                                      //추가
        //추가
        //Run_Road road = roads[Random.Range(0, roads.Count)];
        //roads.Remove(road);
        road.transform.position = spawnPosition;
        road.transform.forward = spawnDir;
        lastRoad = road;
        road.gameObject.SetActive(true);

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
            //gameObject.SetActive(false);
            Debug.Log("아 오브젝트 비활성화 완료");

        //여기서 비활성화한 오브젝트의 자식객체의 Layer를 탐색하여 다시 Tile로 바꿔줌
        Transform parentTranform = this.transform;
        foreach (Transform childTransform in parentTransform)
        {
            if (childTransform.name == "Water")
            {
                childTransform.gameObject.layer = 6;
                print("레이어 6번으로 바뀌었습니다.");
            }
        }

        

    }
    public void ReturnToPool(Run_Road road)
    {
        ReturnList(road.transform,"Tile");
        road.gameObject.SetActive(false);
        activeRoads.Remove(road);
        inactiveRoads.Add(road);
    }

}
