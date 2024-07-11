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
        rend = GetComponentInChildren<MeshRenderer>();
        mat = rend.materials[0];
        col = GetComponent<BoxCollider>();
    }
    protected virtual void Start()
    {
        myLayer = Puzzle_GameManager.instance.TileLayer;
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

    public virtual void TileEvent(Puzzle_Horse_Movement target)
    {
        print($"���� {name} Ÿ���� ��ҽ��ϴ�.");
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
            -1 :
            //Ÿ���� ������ �ִ� ��� �ڽŵ� ������ �� �� �����Ƿ� Ÿ�� ���̾ ������ ��� ���̾ �˻�
            ~myLayer.value;

        Collider[] cols = Physics.OverlapBox(transform.position, Vector3.one * range, Quaternion.identity, targetLayer);
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
    protected virtual void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, Vector3.one * range);
        }
    }
}
