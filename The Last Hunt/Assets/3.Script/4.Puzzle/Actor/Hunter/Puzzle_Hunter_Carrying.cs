using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle_Hunter_Carrying : MonoBehaviour
{
    /// <summary>
    /// 타일을 들고 있지 않을 때 보고 있는 타일을 들 수 있는지 없는지 알려주는 타일
    /// </summary>
    [SerializeField] Puzzle_Guide guideTile;
    [SerializeField] RectTransform guideUI;
    Camera cam;

    Puzzle_Movement movement;

    /// <summary>
    /// 현재 들고 있는 타일
    /// </summary>
    Puzzle_Tile holdingTile;

    private void Awake()
    {
        movement = GetComponent<Puzzle_Hunter_Movement>();
    }

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
        if (movement.ViewingTile)
        {
            // 길 타일 위에 무언가 있을 경우
            // 자신을 제외하고 검사
            if (movement.ViewingTile.IsOverlapping())
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
            holdingTile = movement.ViewingTile;
            holdingTile.Holding();
            guideTile.SetInvisible(false);
        }
    }
    void SetTile()
    {
        //지금 검사하고 있는 위치에 타일이 없으면 설치 가능
        if (movement.ViewingTile == null)
        {
            holdingTile.transform.position = movement.ForwardPosition;
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
        //guideUI.GetComponent<Image>().enabled = false;
        print("떨어짐");
    }

    [SerializeField]
    float holdingHeight = 1; 

    private void LateUpdate()
    {
        Vector3 forward = movement.ForwardPosition;

        Vector2 pos = cam.WorldToScreenPoint(forward);
        guideUI.position = pos;

        //타일을 이미 들고 있는 경우
        if (holdingTile)
        {
            holdingTile.transform.position = forward + Vector3.up * holdingHeight;
        }
        //타일을 들고 있지 않은 경우
        else
        {
            //사냥꾼의 눈 앞에 타일이 있을 때 보인다.
            guideTile.SetInvisible(movement.ViewingTile != null);
            guideTile.transform.position = forward + Vector3.up * holdingHeight;
        }
    }
}
