using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Puzzle_Hunter_TileAction : Puzzle_TileChecker
{
    /// <summary>
    /// ���� �÷��̾ ���� ���� Ÿ��
    /// </summary>
    public Puzzle_Tile HoldingTile { get; private set; }
    /// <summary>
    /// Ÿ���� ��� ���� ���� �� �տ� ������ �ִ� Ÿ���� �� �� �ִ��� ������ �˷��ִ� Ÿ��
    /// </summary>
    Puzzle_Guide guideTile;
    //[SerializeField, Header("Ÿ���� ��� ���� ����"), Space(10)]
    //float holdingHeight = 1;
    [SerializeField, Header("�÷��̾��� �ü��� ǥ����UI"), Space(10)]
    RectTransform guideUI;

    Camera cam;

    void Awake()
    {
        guideTile = FindObjectOfType<Puzzle_Guide>();
        cam = Camera.main;
        HoldingTile = null;
    }

    public void CarryingAction()
    {
        //��� ���� ���� ��
        if (HoldingTile == null)
        {
            TakeTile();
        }
        //��� ���� ��
        else
        {
            SetTile();
        }
    }

    void TakeTile()
    {
        //�����ִ� ���� Ÿ���� ���� ���
        if (ViewingTile)
        {
            //��ɲ��̶� ���� �� Ÿ�� ���� ���� ��� Ÿ���� �̵� ��ų �� �ִ�.
            if (ViewingTile.IsOverlapping())
            {
                print("���� �� �� ���� Ÿ���Դϴ�!");
                return;
            }
            else if (ViewingTile.TryGetComponent(out Puzzle_Road _))
            {
                HoldingTile = ViewingTile;
                HoldingTile.Holding();
                guideTile.SetInvisible(false);
                guideTile.enabled = false;
            }
        }
    }

    void SetTile()
    {
        if (ViewingTile != null) return;

        //���߿� �� ��ü �׵θ��� Dead Tile�� ��ġ�Ͽ� ������ ����
        else if (ForwardPosition.x < -3 ||
            ForwardPosition.z < -9 ||
            ForwardPosition.z > 9) return;

        //���� �˻��ϰ� �ִ� ��ġ�� Ÿ���� ������ ��ġ ����
        else
        {
            HoldingTile.transform.position = ForwardPosition;
            HoldingTile.ResetState();
            HoldingTile = null;
            guideTile.enabled = true;
            guideTile.SetInvisible(true);
        }
    }

    private void FixedUpdate()
    {
        Vector3 forward = ForwardPosition;


        //Ÿ���� �̹� ��� �ִ� ���
        if (HoldingTile)
        {
            HoldingTile.transform.position = forward;
        }
        //Ÿ���� ��� ���� ���� ���
        else
        {
            //��ɲ��� �� �տ� Ÿ���� ���� �� ���δ�.
            guideTile.transform.position = forward;
            guideTile.SetInvisible(ViewingTile != null);
        }
    }
}
