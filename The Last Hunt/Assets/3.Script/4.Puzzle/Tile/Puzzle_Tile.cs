using System.Collections;
using UnityEngine;

public class Puzzle_Tile : MonoBehaviour
{
    public enum TileType
    {
        /// <summary>
        /// ���� �����ϴ� Ÿ��
        /// </summary>
        Straight = 0,
        /// <summary>
        /// �ε����� ���� �״� Ÿ��
        /// </summary>
        Obstacle,
        /// <summary>
        /// ���� ��ȸ���ϴ� Ÿ��
        /// </summary>
        Left,
        /// <summary>
        /// ���� ��ȸ���ϴ� Ÿ��
        /// </summary>
        Right
    }

    public TileType tileType = new TileType();

    Vector3 outPosition;
    Vector3 InPosition;

    MeshRenderer rend;
    Material mat;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        mat = rend.materials[0];
        //mat.SetInt();
    }




    void OnBoardEvent(Transform horse)
    {
        switch (tileType)
        {
            case TileType.Obstacle:
                //�� ����, ���� ó������
                break;
            case TileType.Straight:
                //�ƹ� �ϵ� ����
                break;
            case TileType.Left:
                horse.rotation *= Quaternion.Euler(0, 90f, 0);
                horse.position = transform.position;
                //�� ��ȸ��
                break;
            case TileType.Right:
                horse.rotation *= Quaternion.Euler(0, -90f, 0); 
                horse.position = transform.position;
                break;
        }
    }
}
