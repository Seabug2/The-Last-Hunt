using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Hunter_Input : MonoBehaviour
{
    /// <summary>
    /// x = 검출 앞 거리
    /// y = 검출 높이
    /// </summary>
    [SerializeField]
    Vector2 offset = new Vector2();

    private void OnDrawGizmos()
    {
        Vector3 drawPosition = transform.position + transform.forward * offset.x + Vector3.up * offset.y;

        Gizmos.DrawWireSphere(drawPosition, 1f);
    }

    /// <summary>
    /// 타일을 들고 있는가?
    /// </summary>
    bool isCarrying = false;

    private void Update()
    {
        //플레이어 앞의 특정 위치에서 Raycast를 실행
        Vector3 drawPosition = transform.position + transform.forward * offset.x + Vector3.up * offset.y;

        //아무것도 들고 있지 않을 때 검사를 시작
        if (isCarrying)
        {
            //자신 앞의 특정 거리를 검사
            if()
        }  
    }
}
