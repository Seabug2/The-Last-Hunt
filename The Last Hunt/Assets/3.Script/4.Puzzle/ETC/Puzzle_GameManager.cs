using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Puzzle_GameManager : MonoBehaviour
{
    public static Puzzle_GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    [SerializeField]
    LayerMask tileLayer;

    public LayerMask TileLayer => tileLayer;

    GameObject hunter;
    public GameObject Hunter => hunter;

    GameObject horse;
    public GameObject Horse => horse;

    /// <summary>
    /// �����Ǵ� �� Ÿ��
    /// </summary>
    Puzzle_Road[] roadTiles;

    public void Init()
    {
        roadTiles = FindObjectsOfType<Puzzle_Road>();
        hunter = FindObjectOfType<GameObject>();
        horse = FindObjectOfType<GameObject>();
        //hunter.transform.position = new Vector3(-1.5f, 0, 0);
        //horse.transform.position = new Vector3(0, 0, -3);
    }

    Coroutine StartEvent;
    IEnumerator StartEvent_co()
    {
        //���̵� ��
        //�޼��� ���
        //Ÿ���� �ϳ���...
        List<Puzzle_Road> roads = new List<Puzzle_Road>(roadTiles);

        float t = 1f;

        while(roads.Count > 0)
        {
            Puzzle_Road road = roads[Random.Range(0, roads.Count)];
            roads.Remove(road);
            yield return new WaitForSeconds(t);
            t *= .9f;
        }

        // �÷��̾� ���۰� �̵� Ȱ��ȭ
        // �� �̵� ����
        // Ÿ�̸� Ȱ��ȭ
        // ��Ÿ UI Ȱ��ȭ
    }
}
