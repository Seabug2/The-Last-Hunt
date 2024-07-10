using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle_Hunter_Carrying : MonoBehaviour
{
    //Ÿ���� �ű� �� �ִ�

    /// <summary>
    /// x = ������ �� �� �Ÿ�
    /// </summary>
    [SerializeField]
    float range = 1.5f;

    /// <summary>
    /// �÷��̾� ���� Ư�� ��ġ���� Ray�� ����
    /// </summary>
    Ray FrontRay
    {
        get
        {
            return new Ray(ForwardPosition, Vector3.down);
        }
    }

    /// <summary>
    /// ���� �����ϰ� �ִ� ��ġ�� 3x3 �׸���� ����
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

    [SerializeField, Header("Ÿ�� ���̾�"), Space(10)]
    LayerMask tileLayer;

    /// <summary>
    /// ���� �����ִ� Ÿ��
    /// </summary>
    public Puzzle_Tile ViewingTile
    {
        get
        {
            //Raycast�� �ؼ� Ÿ���� �����ɴϴ�.
            if (Physics.Raycast(FrontRay, out RaycastHit hit, Mathf.Infinity, tileLayer))
            {
                return hit.transform.GetComponent<Puzzle_Tile>();
            }
            return null;
        }
    }

    /// <summary>
    /// Ÿ���� ��� ���� ���� �� ���� �ִ� Ÿ���� �� �� �ִ��� ������ �˷��ִ� Ÿ��
    /// </summary>
    [SerializeField] Puzzle_Guide guideTile;
    [SerializeField] RectTransform guideUI;
    Camera cam;

    /// <summary>
    /// ���� ��� �ִ� Ÿ��
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
        if (ViewingTile)
        {
            // �� Ÿ�� ���� ���� ���� ���
            // �ڽ��� �����ϰ� �˻�
            if (ViewingTile.IsOverlapping())
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
            holdingTile = ViewingTile;
            holdingTile.Holding();
            guideTile.SetInvisible(false);
        }
    }
    void SetTile()
    {
        //���� �˻��ϰ� �ִ� ��ġ�� Ÿ���� ������ ��ġ ����
        if (ViewingTile == null)
        {
            holdingTile.transform.position = new Vector3(ForwardPosition.x, -.25f, ForwardPosition.z);
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
        guideUI.GetComponent<Image>().enabled = false;
        print("������");
    }

    private void LateUpdate()
    {
        Vector2 pos = cam.WorldToScreenPoint(ForwardPosition);
        guideUI.position = pos;

        //Ÿ���� �̹� ��� �ִ� ���
        if (holdingTile)
        {
            holdingTile.transform.position = ForwardPosition;
        }
        //Ÿ���� ��� ���� ���� ���
        else
        {
            //��ɲ��� �� �տ� Ÿ���� ���� �� ���δ�.
            guideTile.SetInvisible(ViewingTile != null);
            guideTile.transform.position = ForwardPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Ÿ���� ������ Ray�� ���� ��ġ
        Gizmos.DrawRay(FrontRay);
    }
}
