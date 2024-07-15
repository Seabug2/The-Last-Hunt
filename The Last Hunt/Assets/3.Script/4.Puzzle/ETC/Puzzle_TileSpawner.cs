using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_TileSpawner : MonoBehaviour
{
    //타일의 크기는?
    int tileSize;
    //맵 크기를 알아야함
    [SerializeField, Header("목적지(집)")]
    Transform home;

    //타일을 저장하고 있어야함
    [SerializeField, Header("타일"), Space(10)]
    //로드타일
    GameObject[] road;
    //데드타일
    [SerializeField]
    GameObject dead;

    [Header("아이템과 장애물"), Space(10)]
    [SerializeField, Header("2 : 칼"), Header("1 : 곡괭이"), Header("0 : 도끼")]
    GameObject[] items;
    [SerializeField, Header("2 : 늑대"), Header("1 : 바위"), Header("0 : 나무")]
    GameObject[] obstacle;

    private void Start()
    {
        tileSize = Puzzle_GameManager.instance.tileSize;
    }

    public void LevelSetUp()
    {
        int i = 12 + tileSize;
        int homePosition = Mathf.RoundToInt(home.position.x - tileSize);
        while (i < homePosition)
        {
            i += tileSize * (Random.Range(0, 2) == 0 ? SetDeadTile(i) : SetRoadTile(i));
        }
    }

    int SetDeadTile(int i)
    {
        Instantiate(dead, new Vector3(i, 0, Mathf.Round(Random.Range(-9, 9) / tileSize) * tileSize), Quaternion.identity);
        return 1;
    }

    int SetRoadTile(int i)
    {
        int pattern = Random.Range(0, items.Length);

        GameObject randomRoad = Instantiate(road[Random.Range(0, road.Length)]);
        randomRoad.transform.position = new Vector3(i, 0, Mathf.Round(Random.Range(-9, 9) / tileSize) * tileSize);
        GameObject _item = Instantiate(items[pattern], randomRoad.transform);
        _item.name = items[pattern].name;
        _item.transform.localPosition = Vector3.zero;

        randomRoad = Instantiate(road[Random.Range(0, road.Length)]);
        randomRoad.transform.position = new Vector3(i + tileSize, 0, Mathf.Round(Random.Range(-9, 9) / tileSize) * tileSize);
        Instantiate(obstacle[pattern], randomRoad.transform);
        return 2;
    }

#if UNITY_EDITOR
    [Space(10)]
    public bool showGizmo;
    const int textSize = 30;
    protected virtual void OnDrawGizmos()
    {
        if (showGizmo)
        {
            GUIStyle style = new GUIStyle
            {
                fontSize = textSize
            };
            style.normal.textColor = Color.red;
            UnityEditor.Handles.Label(transform.position, $"{transform.position.x}, {transform.position.z}", style);
        }
    }
#endif
}
