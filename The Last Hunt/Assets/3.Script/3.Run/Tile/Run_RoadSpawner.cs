using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_RoadSpawner : MonoBehaviour
{

    [SerializeField] List<Run_Road> roads = new List<Run_Road>();
    
    Run_Road lastRoad;

    private List<Run_Road> activeRoads = new List<Run_Road>();                 //�߰�
    private List<Run_Road> inactiveRoads = new List<Run_Road>();               //�߰�


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
    /// ������ ��ġ��, ������ �������� ������ ���� ���� �÷��̾ ��� �ִ� �� ������Ʈ �տ� �����մϴ�.
    /// </summary>
    public  void InstantiateRoad(Vector3 spawnPosition, Vector3 spawnDir)
    {
        int randomIndex = Random.Range(0, inactiveRoads.Count);     //�߰�
        Run_Road road = inactiveRoads[randomIndex];                 //�߰�
        inactiveRoads.RemoveAt(randomIndex);                        //�߰�
        activeRoads.Add(road);                                      //�߰�
        //�߰�
        //Run_Road road = roads[Random.Range(0, roads.Count)];
        //roads.Remove(road);
        road.transform.position = spawnPosition;
        road.transform.forward = spawnDir;
        lastRoad = road;
        road.gameObject.SetActive(true);

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
            //gameObject.SetActive(false);
            Debug.Log("�� ������Ʈ ��Ȱ��ȭ �Ϸ�");

        //���⼭ ��Ȱ��ȭ�� ������Ʈ�� �ڽİ�ü�� Layer�� Ž���Ͽ� �ٽ� Tile�� �ٲ���
        Transform parentTranform = this.transform;
        foreach (Transform childTransform in parentTransform)
        {
            if (childTransform.name == "Water")
            {
                childTransform.gameObject.layer = 6;
                print("���̾� 6������ �ٲ�����ϴ�.");
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
