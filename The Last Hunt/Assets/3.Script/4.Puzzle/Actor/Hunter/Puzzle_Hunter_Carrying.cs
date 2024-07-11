using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle_Hunter_Carrying : MonoBehaviour
{
    /// <summary>
    /// Ÿ���� ��� ���� ���� �� ���� �ִ� Ÿ���� �� �� �ִ��� ������ �˷��ִ� Ÿ��
    /// </summary>
    [SerializeField] Puzzle_Guide guideTile;
    [SerializeField] RectTransform guideUI;
    Camera cam;

    Puzzle_Movement movement;

    /// <summary>
    /// ���� ��� �ִ� Ÿ��
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
        //���� �˻��ϰ� �ִ� ��ġ�� Ÿ���� ������ ��ġ ����
        if (movement.ViewingTile == null)
        {
            holdingTile.transform.position = movement.ForwardPosition;
            holdingTile.ResetState();
            holdingTile = null;

            guideTile.SetInvisible(true);
        }
        //�̹� Ÿ���� �ִ� �ڸ����� ��ġ �Ұ���
        else
        {
            print("���� ��ġ�� �̹� Ÿ���� �ֽ��ϴ�.");
        }
    }

    private void OnDisable()
    {
        //guideUI.GetComponent<Image>().enabled = false;
        print("������");
    }

    [SerializeField]
    float holdingHeight = 1; 

    private void LateUpdate()
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
