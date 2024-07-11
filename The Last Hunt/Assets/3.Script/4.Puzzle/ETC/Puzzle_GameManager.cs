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
    /// 제공되는 길 타일
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
        //페이드 인
        //메세지 출력
        //타일을 하나씩...
        List<Puzzle_Road> roads = new List<Puzzle_Road>(roadTiles);

        float t = 1f;

        while(roads.Count > 0)
        {
            Puzzle_Road road = roads[Random.Range(0, roads.Count)];
            roads.Remove(road);
            yield return new WaitForSeconds(t);
            t *= .9f;
        }

        // 플레이어 조작과 이동 활성화
        // 말 이동 시작
        // 타이머 활성화
        // 기타 UI 활성화
    }
}
