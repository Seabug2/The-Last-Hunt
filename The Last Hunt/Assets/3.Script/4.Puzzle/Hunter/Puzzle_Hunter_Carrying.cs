using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Hunter_Carrying : MonoBehaviour
{
    /// <summary>
    /// x = 검출 앞 거리
    /// y = 검출 높이
    /// </summary>
    [SerializeField]
    Vector2 offset = new Vector2();
    /// <summary>
    /// 타일을 들고 있는가?
    /// </summary>
    bool isCarrying = false;

    Puzzle_Tile tile = null;

    [SerializeField]
    LayerMask tileLayer;

    [SerializeField]
    Transform tileMark;
    MeshRenderer tileMarkRend;

    Transform selectedTile;

    private void Awake()
    {
        tileMarkRend = tileMark.GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// 플레이어 앞의 특정 위치에서 Ray를 생성
    /// </summary>
    Ray FrontRay
    {
        get
        {
            Vector3 origin = transform.position + transform.forward * offset.x + Vector3.up * offset.y;
            Vector3 direction = origin + Vector3.down;
            return new Ray(origin, direction);
        }
    }

    public void CarryingAction()
    {
        TileCheck();
        //들고 있을 때
        if (isCarrying)
        {
            SetTile();
        }
        //들고 있지 않을 때
        else
        {
            TakeTile();
        }
    }

    void SetTile()
    {

    }
    void TakeTile()
    {

    }

    Puzzle_Tile TileCheck()
    {
        if (Physics.Raycast(FrontRay, out RaycastHit hit, 10, tileLayer))
        {
            print(hit.transform.name);
        }

        return null;
    }

    private void Update()
    {
        if (isCarrying)
        {
            tileMarkRend.enabled = false;
        }
        else
        {
            float x = Mathf.Round(FrontRay.origin.x / 3) * 3;
            float z = Mathf.Round(FrontRay.origin.z /3) * 3;
            tileMark.position = new Vector3(x, -.25f, z);
            if(!tileMarkRend.enabled)
                tileMarkRend.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        // 타일을 검출할 Ray의 시작 위치
        Gizmos.DrawWireSphere(FrontRay.origin, 0.1f);
    }
}
