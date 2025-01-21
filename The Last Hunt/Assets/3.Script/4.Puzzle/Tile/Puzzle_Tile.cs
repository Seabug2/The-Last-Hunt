using UnityEngine;

public class Puzzle_Tile : MonoBehaviour
{
    //Ÿ�� Ŭ�������� �����ؾ� �ϴ� ��
    protected MeshRenderer rend;
    protected Material mat;
    protected BoxCollider col;

    [SerializeField]
    protected LayerMask ignoreLayer;

    /// <summary>
    /// �ݶ��̴��� ��Ȱ��ȭ ���¶�� ��� �ִ� �����̴�.
    /// </summary>
    public bool IsHolding
    {
        get
        {
            return col.enabled == false;
        }
    }

    protected virtual void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        mat = rend.materials[0];
        col = GetComponent<BoxCollider>();
    }

    protected virtual void Start()
    {
        ignoreLayer = Puzzle_GameManager.instance.TileLayer;
    }

    public virtual void ResetState()
    {
        //���������� �ٲ�
        mat.SetInt("_IsHolding", 0);
        mat.SetInt("_IsOverlapping", 0);
        mat.renderQueue = 3000;
        col.enabled = true;
    }

    public void Holding()
    {
        //���������� �ٲ�
        mat.SetInt("_IsHolding", 1);
        mat.renderQueue = 3500;
        col.enabled = false;
    }

    //Ÿ���� ��� ������ ���� ����
    public virtual void OnStepped(Puzzle_Horse target)
    {
        print($"���� {name} Ÿ���� ��ҽ��ϴ�.");
    }

    public void Overlap(bool _isOverlap)
    {
        mat.SetInt("_IsOverlapping", _isOverlap ? 1 : 0);
    }

    protected const float range = .9f;

    /// <summary>
    /// ���� Ÿ���� ��ġ���� ����Ǵ� Ÿ�ϵ��� ��ȯ
    /// </summary>
    public bool IsOverlapping()
    {
        int targetLayer = IsHolding ?
            //Ÿ���� ��� �ִ� ��� Ÿ���� �ڽ��� ���� ���� �����Ƿ� ��� ���̾��� ������Ʈ�� �˻�
            -1 :
            //Ÿ���� ������ �ִ� ��� �ڽŵ� ������ �� �� �����Ƿ� Ÿ�� ���̾ ������ ��� ���̾ �˻�
            ~ignoreLayer.value;

        bool overlapedTile= Physics.CheckBox(transform.position, Vector3.one * range, Quaternion.identity, targetLayer);
        if (overlapedTile)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
#if UNITY_EDITOR
    [Space(10)]
    public bool showGizmo;
    const int textSize = 30;
    protected virtual void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, Vector3.one * range);
            GUIStyle style = new GUIStyle
            {
                fontSize = textSize
            };
            style.normal.textColor = Color.red;
            UnityEditor.Handles.Label(transform.position, $"{transform.position.x}, {transform.position.z}", style);
        }
    }
#endif
}
