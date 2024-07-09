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


    [SerializeField]
    LayerMask tileLayer;

    [SerializeField]
    Puzzle_Guide guideTile;

    Puzzle_Tile selectedTile;

    float tilePosY = 0.25f;
    private void Awake()
    {
        tilePosY = -transform.lossyScale.y * .5f;
    }

    private void OnEnable()
    {
        isCarrying = false;
    }

    /// <summary>
    /// 플레이어 앞의 특정 위치에서 Ray를 생성
    /// </summary>
    Ray FrontRay
    {
        get
        {
            Vector3 origin = transform.position + transform.forward * offset.x + Vector3.up * offset.y;
            Vector3 direction = origin + Vector3.down * rayDown;
            return new Ray(origin, direction);
        }
    }

    const float rayDown = 3f;

    public void CarryingAction()
    {
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

    //타일 내려두기의 조건
    void SetTile()
    {
        isCarrying = false;
    }

    //타일 들기의 조건
    void TakeTile()
    {
        print("타일을 든다");

        //눈 앞의 타일을 가져온다
        selectedTile = TileCheck();
        if (selectedTile != null)
        {
            print(selectedTile.name);

            //selectedTile.gameObject.SetActive(false);
            isCarrying = true;
        }
    }

    Puzzle_Tile TileCheck()
    {
        //Raycast를 해서 타일을 가져옵니다.
        if (Physics.Raycast(FrontRay, out RaycastHit hit, rayDown, tileLayer))
        {
            print(hit.transform.name);
            //만약 타일 위에 아무것도 없을 경우
            //if (!tile.isGrounded)
            return hit.transform.GetComponent<Puzzle_Tile>();
        }

        return null;
    }

    private void Update()
    {
        ShowGuide();
    }

    void ShowGuide()
    {
        guideTile.transform.position = SetPosition();
    }

    public Vector3 SetPosition()
    {
        float x = Mathf.Round(FrontRay.origin.x / 3) * 3;
        float z = Mathf.Round(FrontRay.origin.z / 3) * 3;
        return new Vector3(x, tilePosY, z);
    }

    private void OnDrawGizmos()
    {
        // 타일을 검출할 Ray의 시작 위치
        Gizmos.DrawLine(FrontRay.origin, FrontRay.origin + Vector3.down * rayDown);
    }
}
