using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_TileSpawner : MonoBehaviour
{
    //Ÿ���� ũ���?
    int tileSize;
    //�� ũ�⸦ �˾ƾ���
    [SerializeField, Header("������(��)")]
    Transform home;

    //Ÿ���� �����ϰ� �־����
    [SerializeField, Header("Ÿ��"), Space(10)]
    //�ε�Ÿ��
    GameObject[] road;
    //����Ÿ��
    [SerializeField]
    GameObject dead;

    [Header("�����۰� ��ֹ�"), Space(10)]
    [SerializeField, Header("2 : Į"), Header("1 : ���"), Header("0 : ����")]
    GameObject[] items;
    [SerializeField, Header("2 : ����"), Header("1 : ����"), Header("0 : ����")]
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
