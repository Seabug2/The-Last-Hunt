using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Hunter_Carrying : MonoBehaviour
{
    /// <summary>
    /// x = ���� �� �Ÿ�
    /// y = ���� ����
    /// </summary>
    [SerializeField]
    Vector2 offset = new Vector2();
    /// <summary>
    /// Ÿ���� ��� �ִ°�?
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
    /// �÷��̾� ���� Ư�� ��ġ���� Ray�� ����
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
        //��� ���� ��
        if (isCarrying)
        {
            SetTile();
        }
        //��� ���� ���� ��
        else
        {
            TakeTile();
        }
    }

    //Ÿ�� �����α��� ����
    void SetTile()
    {
        isCarrying = false;
    }

    //Ÿ�� ����� ����
    void TakeTile()
    {
        print("Ÿ���� ���");

        //�� ���� Ÿ���� �����´�
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
        //Raycast�� �ؼ� Ÿ���� �����ɴϴ�.
        if (Physics.Raycast(FrontRay, out RaycastHit hit, rayDown, tileLayer))
        {
            print(hit.transform.name);
            //���� Ÿ�� ���� �ƹ��͵� ���� ���
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
        // Ÿ���� ������ Ray�� ���� ��ġ
        Gizmos.DrawLine(FrontRay.origin, FrontRay.origin + Vector3.down * rayDown);
    }
}
