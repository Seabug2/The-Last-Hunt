using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle_Hunter_Carrying : MonoBehaviour
{
    //타일을 옮길 수 있다

    /// <summary>
    /// x = 검출할 앞 쪽 거리
    /// </summary>
    [SerializeField]
    float range = 1.5f;

    /// <summary>
    /// 플레이어 앞의 특정 위치에서 Ray를 생성
    /// </summary>
    Ray FrontRay
    {
        get
        {
            return new Ray(ForwardPosition, Vector3.down);
        }
    }

    /// <summary>
    /// 현재 검출하고 있는 위치를 3x3 그리드로 설정
    /// </summary>
    public Vector3 ForwardPosition
    {
        get
        {
            Vector3 origin = transform.position + transform.forward * range;
            float x = Mathf.Round(origin.x / 3) * 3;
            float z = Mathf.Round(origin.z / 3) * 3;
            return new Vector3(x, transform.position.y, z);
        }
    }

    [SerializeField, Header("타일 레이어"), Space(10)]
    LayerMask tileLayer;

    /// <summary>
    /// 현재 보고있는 타일
    /// </summary>
    public Puzzle_Tile ViewingTile
    {
        get
        {
            //Raycast를 해서 타일을 가져옵니다.
            if (Physics.Raycast(FrontRay, out RaycastHit hit, Mathf.Infinity, tileLayer))
            {
                return hit.transform.GetComponent<Puzzle_Tile>();
            }
            return null;
        }
    }

    /// <summary>
    /// 타일을 들고 있지 않을 때 보고 있는 타일을 들 수 있는지 없는지 알려주는 타일
    /// </summary>
    [SerializeField] Puzzle_Guide guideTile;
    [SerializeField] RectTransform guideUI;
    Camera cam;

    /// <summary>
    /// 현재 들고 있는 타일
    /// </summary>
    [SerializeField] Puzzle_Tile holdingTile;

    private void Start()
    {
        holdingTile = null;
        if (!guideTile)
        {
            guideTile = FindObjectOfType<Puzzle_Guide>();
        }
        guideTile.SetInvisible(true);
        cam = Camera.main;
    }

    public void CarryingAction()
    {
        //들고 있지 않을 때
        if (holdingTile == null)
        {
            TakeTile();
        }
        //들고 있을 때
        else
        {
            SetTile();
        }
    }

    void TakeTile()
    {
        //보고있는 곳에 타일이 있을 경우
        if (ViewingTile)
        {
            // 길 타일 위에 무언가 있을 경우
            // 자신을 제외하고 검사
            if (ViewingTile.IsOverlapping())
            {
                print("타일 위에 무언가 있습니다!");
                return;
            }
            // 길 타일이 아닌 경우
            //else if()
            //{
            // return;
            //}

            //사냥꾼이랑 말이 그 타일 위에 없는 경우 타일을 이동 시킬 수 있다.
            holdingTile = ViewingTile;
            holdingTile.Holding();
            guideTile.SetInvisible(false);
        }
    }
    void SetTile()
    {
        //지금 검사하고 있는 위치에 타일이 없으면 설치 가능
        if (ViewingTile == null)
        {
            holdingTile.transform.position = new Vector3(ForwardPosition.x, -.25f, ForwardPosition.z);
            holdingTile.ResetState();
            holdingTile = null;

            guideTile.SetInvisible(true);
        }
        //이미 타일이 있는 자리에는 설치 불가능
        else
        {
            print("현재 위치에 이미 타일이 있습니다.");
        }
    }

    private void OnDisable()
    {
        guideUI.GetComponent<Image>().enabled = false;
        print("떨어짐");
    }

    private void LateUpdate()
    {
        Vector2 pos = cam.WorldToScreenPoint(ForwardPosition);
        guideUI.position = pos;

        //타일을 이미 들고 있는 경우
        if (holdingTile)
        {
            holdingTile.transform.position = ForwardPosition;
        }
        //타일을 들고 있지 않은 경우
        else
        {
            //사냥꾼의 눈 앞에 타일이 있을 때 보인다.
            guideTile.SetInvisible(ViewingTile != null);
            guideTile.transform.position = ForwardPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // 타일을 검출할 Ray의 시작 위치
        Gizmos.DrawRay(FrontRay);
    }
}
