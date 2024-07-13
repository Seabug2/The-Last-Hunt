using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Puzzle_Hunter_TileAction : Puzzle_TileChecker
{
    /// <summary>
    /// 현재 플레이어가 선택 중인 타일
    /// </summary>
    Puzzle_Tile holdingTile;
    /// <summary>
    /// 타일을 들고 있지 않을 때 보고 있는 타일을 들 수 있는지 없는지 알려주는 타일
    /// </summary>
    Puzzle_Guide guideTile;
    //[SerializeField, Header("타일을 들고 있을 높이"), Space(10)]
    //float holdingHeight = 1;
    [SerializeField, Header("플레이어의 시선을 표시할UI"), Space(10)]
    RectTransform guideUI;

    Camera cam;

    void Awake()
    {
        guideTile = FindObjectOfType<Puzzle_Guide>();
        cam = Camera.main;
        holdingTile = null;
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
            //사냥꾼이랑 말이 그 타일 위에 없는 경우 타일을 이동 시킬 수 있다.
            if (ViewingTile.IsOverlapping())
            {
                print("현재 들 수 없는 타일입니다!");
                return;
            }
            else if (ViewingTile.TryGetComponent(out Puzzle_Road _))
            {
                holdingTile = ViewingTile;
                holdingTile.Holding();
                guideTile.SetInvisible(false);
                guideTile.enabled = false;
            }
        }
    }

    void SetTile()
    {
        if (ViewingTile != null) return;

        //나중에 맵 전체 테두리에 Dead Tile을 설치하여 생략할 예정
        else if (ForwardPosition.x < -3 ||
            ForwardPosition.z < -9 ||
            ForwardPosition.z > 9) return;

        //지금 검사하고 있는 위치에 타일이 없으면 설치 가능
        else
        {
            holdingTile.transform.position = ForwardPosition;
            holdingTile.ResetState();
            holdingTile = null;
            guideTile.enabled = true;
            guideTile.SetInvisible(true);
        }
    }

    private void FixedUpdate()
    {
        Vector3 forward = ForwardPosition;

        Vector2 pos = cam.WorldToScreenPoint(forward);
        guideUI.position = pos;

        //타일을 이미 들고 있는 경우
        if (holdingTile)
        {
            holdingTile.transform.position = forward;
        }
        //타일을 들고 있지 않은 경우
        else
        {
            //사냥꾼의 눈 앞에 타일이 있을 때 보인다.
            guideTile.transform.position = forward;
            guideTile.SetInvisible(ViewingTile != null);
        }
    }
}
