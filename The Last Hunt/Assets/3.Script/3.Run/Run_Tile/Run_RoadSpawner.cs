using System.Collections.Generic;
using UnityEngine;

public class Run_RoadSpawner : MonoBehaviour
{
    /// <summary>
    /// 마지막으로 생성한 길
    /// </summary>
    [SerializeField] Run_Road lastRoad = null;
    /// <summary>
    /// 비활성화된 생성 대기중인 길
    /// </summary>
    [SerializeField] List<Run_Road> inactiveRoads = new List<Run_Road>();

    /// <summary>
    /// 지정한 위치에, 지정한 방향으로 랜덤한 길을 현재 플레이어가 밟고 있는 길 오브젝트 앞에 생성합니다.
    /// </summary>
    public  void LoadRoad(Run_Road calledRoad)
    {
        lastRoad.gameObject.SetActive(false);
        inactiveRoads.Add(lastRoad);

        lastRoad = calledRoad;

        int randomIndex = Random.Range(0, inactiveRoads.Count);
        Run_Road road = inactiveRoads[randomIndex];
        inactiveRoads.Remove(road);

        Vector3 spawnPosition = calledRoad.NextRoadPosition.position;
        road.transform.position = spawnPosition;
        Vector3 spawnDir = calledRoad.NextRoadPosition.forward;
        road.transform.forward = spawnDir;

        road.Setup();

        road.gameObject.SetActive(true);
    }
}
