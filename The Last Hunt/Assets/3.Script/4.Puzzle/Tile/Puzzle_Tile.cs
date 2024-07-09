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


    /// <summary>
    /// 플레이어는 말이 위에 없는 타일을 들어서 옮길 수 있다.
    /// </summary>
    public bool isGrounded;

    private void OnEnable()
    {
        SetRay();
    }

    Ray ray = new Ray();

    public void SetRay()
    {
        ray.origin = transform.position;
        ray.direction = transform.up;
    }

    bool IsOnboard()
    {
        return Physics.Raycast(ray, out RaycastHit hit, 1) && hit.transform.name == "Horse";
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
