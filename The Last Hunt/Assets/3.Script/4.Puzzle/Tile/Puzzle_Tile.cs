using UnityEngine;

public class Puzzle_Tile : MonoBehaviour
{
    //Ÿ�� Ŭ�������� �����ؾ� �ϴ� ��
    protected MeshRenderer rend;
    protected Material mat;
    BoxCollider col;
    
    [SerializeField]
    protected LayerMask myLayer = 0;

    public bool IsHolding
    {
        get
        {
            //�ݶ��̴��� ��Ȱ��ȭ ���¶�� ��� �ִ� �����̴�.
            return col.enabled == false;
        }
    }

    protected virtual void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        mat = rend.materials[0];
        col = GetComponent<BoxCollider>();
        myLayer = 1 << gameObject.layer;
    }

    public virtual void ResetState()
    {
        //���������� �ٲ�
        mat.SetInt("_IsHolding", 0);
        mat.SetInt("_IsOverlapping", 0);
        col.enabled = true;
    }

    public virtual void Holding()
    {
        //���������� �ٲ�
        mat.SetInt("_IsHolding", 1);
        col.enabled = false;
    }
    public void Overlap(bool _isOverlap)
    {
        mat.SetInt("_IsOverlapping", _isOverlap ? 1 : 0);
    }

    protected const float range = 1f;

    /// <summary>
    /// ���� Ÿ���� ��ġ���� ����Ǵ� Ÿ�ϵ��� ��ȯ
    /// </summary>
    public bool IsOverlapping()
    {
        int targetLayer = IsHolding ?
            //Ÿ���� ��� �ִ� ��� Ÿ���� �ڽ��� ���� ���� �����Ƿ� ��� ���̾��� ������Ʈ�� �˻�
            -1:
            //Ÿ���� ������ �ִ� ��� �ڽŵ� ������ �� �� �����Ƿ� Ÿ�� ���̾ ������ ��� ���̾ �˻�
            ~myLayer.value;

        Collider[] cols = Physics.OverlapBox(transform.position, Vector3.one * range,Quaternion.identity, targetLayer);
        if (cols.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [Space(10)]
    public bool showGizmo;
    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, Vector3.one * range);
        }
    }
}
