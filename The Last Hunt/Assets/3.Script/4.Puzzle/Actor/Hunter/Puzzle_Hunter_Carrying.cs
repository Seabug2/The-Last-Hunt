using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle_Hunter_Carrying : MonoBehaviour
{
    /// <summary>
    /// ���� �÷��̾ ���� ���� Ÿ��
    /// </summary>
    Puzzle_Tile holdingTile;
    /// <summary>
    /// Ÿ���� ��� ���� ���� �� ���� �ִ� Ÿ���� �� �� �ִ��� ������ �˷��ִ� Ÿ��
    /// </summary>
    [SerializeField, Header("������ ���̵� Ÿ��"), Space(10)]
    Puzzle_Guide guideTile;
    [SerializeField, Header("Ÿ���� ��� ���� ����"), Space(10)]
    float holdingHeight = 1;
    [SerializeField, Header("�÷��̾��� �ü��� ǥ����UI"), Space(10)]
    RectTransform guideUI;

    Camera cam;
    Puzzle_Movement movement;

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
        //��� ���� ���� ��
        if (holdingTile == null)
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
        if (movement.ViewingTile)
        {
            // �� Ÿ�� ���� ���� ���� ���
            // �ڽ��� �����ϰ� �˻�
            if (movement.ViewingTile.IsOverlapping())
            {
                print("Ÿ�� ���� ���� �ֽ��ϴ�!");
                return;
            }
            // �� Ÿ���� �ƴ� ���
            //else if()
            //{
            // return;
            //}

            //��ɲ��̶� ���� �� Ÿ�� ���� ���� ��� Ÿ���� �̵� ��ų �� �ִ�.
            holdingTile = movement.ViewingTile;
            holdingTile.Holding();
            guideTile.SetInvisible(false);
        }
    }

    void SetTile()
    {
        if (movement.ViewingTile != null) return;
        else if (movement.ForwardPosition.x < -3 ||
            movement.ForwardPosition.z < -9 ||
            movement.ForwardPosition.z > 9) return;

        //���� �˻��ϰ� �ִ� ��ġ�� Ÿ���� ������ ��ġ ����
        else
        {
            holdingTile.transform.position = movement.ForwardPosition;
            holdingTile.ResetState();
            holdingTile = null;
            guideTile.SetInvisible(true);
        }
    }

    private void FixedUpdate()
    {
        Vector3 forward = movement.ForwardPosition;

        Vector2 pos = cam.WorldToScreenPoint(forward);
        guideUI.position = pos;

        //Ÿ���� �̹� ��� �ִ� ���
        if (holdingTile)
        {
            holdingTile.transform.position = forward + Vector3.up * holdingHeight;
        }
        //Ÿ���� ��� ���� ���� ���
        else
        {
            //��ɲ��� �� �տ� Ÿ���� ���� �� ���δ�.
            guideTile.SetInvisible(movement.ViewingTile != null);
            guideTile.transform.position = forward + Vector3.up * holdingHeight;
        }
    }
}
