using UnityEngine;

public class Run_Road : MonoBehaviour
{
    [SerializeField] Transform nextRoadPosition;
    public Transform NextRoadPosition => nextRoadPosition;

    [SerializeField] Transform obstacleRoad;

    Run_RoadSpawner spawner;

    private void Awake()
    {
        spawner = FindObjectOfType<Run_RoadSpawner>();
    }

    public void Init()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.layer = gameObject.layer;
        }
    }

    public void Setup()
    {
        Init();

        int randomIndex = Random.Range(0, obstacleRoad.transform.childCount);

        //바닥이 없는 장애물
        if (Random.Range(0, 2) == 0)
            obstacleRoad.gameObject.SetActive(false);
        else
        {
            for (int i = 0; i < obstacleRoad.transform.childCount; i++)
            {
                if (i == randomIndex)
                    obstacleRoad.GetChild(i).gameObject.SetActive(true);
                else
                    obstacleRoad.GetChild(i).gameObject.SetActive(false);
            }
            obstacleRoad.gameObject.SetActive(true);
        }
    }
}
