using UnityEngine;

public class Puzzle_Tile : MonoBehaviour
{
    //타일 클래스에서 구현해야 하는 것
    protected MeshRenderer rend;
    protected Material mat;
    protected BoxCollider col;

    [SerializeField]
    protected LayerMask ignoreLayer;

    /// <summary>
    /// 콜라이더가 비활성화 상태라면 들고 있는 상태이다.
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
        //불투명으로 바꿈
        mat.SetInt("_IsHolding", 0);
        mat.SetInt("_IsOverlapping", 0);
        mat.renderQueue = 3000;
        col.enabled = true;
    }

    public void Holding()
    {
        //반투명으로 바꿈
        mat.SetInt("_IsHolding", 1);
        mat.renderQueue = 3500;
        col.enabled = false;
    }

    //타일을 상속 구조로 만든 이유
    public virtual void OnStepped(Puzzle_Horse target)
    {
        print($"말이 {name} 타일을 밟았습니다.");
    }

    public void Overlap(bool _isOverlap)
    {
        mat.SetInt("_IsOverlapping", _isOverlap ? 1 : 0);
    }

    protected const float range = .9f;

    /// <summary>
    /// 현재 타일의 위치에서 검출되는 타일들을 반환
    /// </summary>
    public bool IsOverlapping()
    {
        int targetLayer = IsHolding ?
            //타일을 들고 있는 경우 타일은 자신은 검출 되지 않으므로 모든 레이어의 오브젝트를 검사
            -1 :
            //타일이 놓여져 있는 경우 자신도 검출이 될 수 있으므로 타일 레이어를 제외한 모든 레이어를 검사
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
