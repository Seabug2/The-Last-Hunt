using System.Collections.Generic;
using UnityEngine;

public class Run_RoadSpawner : MonoBehaviour
{
    /// <summary>
    /// ���������� ������ ��
    /// </summary>
    [SerializeField] Run_Road lastRoad = null;
    /// <summary>
    /// ��Ȱ��ȭ�� ���� ������� ��
    /// </summary>
    [SerializeField] List<Run_Road> inactiveRoads = new List<Run_Road>();

    /// <summary>
    /// ������ ��ġ��, ������ �������� ������ ���� ���� �÷��̾ ��� �ִ� �� ������Ʈ �տ� �����մϴ�.
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
