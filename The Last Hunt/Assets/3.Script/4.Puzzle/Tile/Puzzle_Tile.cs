using System.Collections;
using UnityEngine;

public class Puzzle_Tile : MonoBehaviour
{
    public enum TileType
    {
        /// <summary>
        /// 말이 직진하는 타일
        /// </summary>
        Straight = 0,
        /// <summary>
        /// 부딪히면 말이 죽는 타일
        /// </summary>
        Obstacle,
        /// <summary>
        /// 말이 좌회전하는 타일
        /// </summary>
        Left,
        /// <summary>
        /// 말이 우회전하는 타일
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
                //말 죽음, 게임 처음으로
                break;
            case TileType.Straight:
                //아무 일도 없음
                break;
            case TileType.Left:
                horse.rotation *= Quaternion.Euler(0, 90f, 0);
                horse.position = transform.position;
                //말 좌회전
                break;
            case TileType.Right:
                horse.rotation *= Quaternion.Euler(0, -90f, 0); 
                horse.position = transform.position;
                break;
        }
    }
}
