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
    /// ������ ��ġ��, ������ �������� ������ ���� ���� �÷��̾ ��� �ִ� �� ������Ʈ �տ� �����մϴ�.
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
        Debug.Log("returnList�� ���Խ��ϴ�.");
        int layer = LayerMask.NameToLayer(layerName);

        if(layer ==-1)
        {
            Debug.Log($"Layer '{layerName}'�� �����ϴ�.");
        }
        //���⼭�� ����Ʈ�� �ٽ� ���ư��� ��길 ���ٲ�
        
            Debug.Log("�� ������Ʈ ��Ȱ��ȭ �غ�");
        this.gameObject.SetActive(false);
            Debug.Log("�� ������Ʈ ��Ȱ��ȭ �Ϸ�");

        //���⼭ ��Ȱ��ȭ�� ������Ʈ�� �ڽİ�ü�� Layer�� Ž���Ͽ� �ٽ� Tile�� �ٲ���
        foreach (Transform childTransform in parentTransform)
        {
            if (childTransform.name == "Water")
            {
                childTransform.gameObject.layer = 6;
                
            }

        }
        
    }
}
