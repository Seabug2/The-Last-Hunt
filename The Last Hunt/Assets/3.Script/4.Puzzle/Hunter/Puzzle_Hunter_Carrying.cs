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
    /// �÷��̾� ���� Ư�� ��ġ���� Ray�� ����
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
        // Ÿ���� ������ Ray�� ���� ��ġ
        Gizmos.DrawWireSphere(FrontRay.origin, 0.1f);
    }
}
