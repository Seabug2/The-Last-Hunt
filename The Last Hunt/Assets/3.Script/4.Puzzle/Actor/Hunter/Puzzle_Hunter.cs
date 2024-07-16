using UnityEngine;

public class Puzzle_Hunter : MonoBehaviour
{
    public Puzzle_Hunter_TileAction TileChecker { get; private set; }
    public Puzzle_Hunter_Movement Movement { get; private set; }
    public Puzzle_Hunter_Input Input { get; private set; }
    public Animator Anim { get; private set; }



    void Awake()
    {
        TileChecker = GetComponent<Puzzle_Hunter_TileAction>();
        Movement = GetComponent<Puzzle_Hunter_Movement>();
        Input = GetComponent<Puzzle_Hunter_Input>();
        Anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //게임 시작시 hand에 준비한 아이템들을 비활성화하여 보이지 않게 합니다.
        foreach (Transform child in hand)
        {
            child.gameObject.SetActive(false);
        }
        EquippedItem = null;

        LockControl();

        Puzzle_GameManager.instance.GameStartEvent.AddListener(() =>
        {
            UnlockControl();
        });
        Puzzle_GameManager.instance.GameClearEvent.AddListener(() =>
        {
            UnequipItem();
            LockControl();
            Anim.SetTrigger("Clear");
        });
        TileChecker.FallingEvent.AddListener(() =>
        {
            UnequipItem();
            LockControl();
            Anim.SetTrigger("Falling");
        });
    }

    /// <summary>
    /// 현재 장착 중인 아이템
    /// </summary>
    public GameObject EquippedItem { get; private set; }

    [SerializeField, Header("무기를 장착할 피벗 위치")]
    Transform hand;
    public Transform EquippedHand => hand;

    //DFS 재귀함수
    void FindTargetTransform(Transform _root, string _name)
    {
        foreach (Transform t in _root)
        {
            if (hand != null) return;
            else if (t.name.Equals(_name))
            {
                hand = t;
                print(_name + "를 찾았습니다!");

                foreach (Transform child in t)
                {
                    child.gameObject.SetActive(false);
                }

                return;
            }
            else if (t.childCount > 0)
            {
                FindTargetTransform(t, _name);
            }
        }
    }

    /// <summary>
    /// 지정한 이름의 장비를 찾아 장착합니다.
    /// </summary>
    /// <param name="_PickUpItemName"></param>
    public void EquipItem(string _PickUpItemName)
    {
        GameObject pickUpItem = hand.Find(_PickUpItemName).gameObject;
        pickUpItem.SetActive(true);
        EquippedItem = pickUpItem;
    }
    /// <summary>
    /// 아이템을 사용하고 착용 해제합니다
    /// </summary>
    public void UnequipItem()
    {
        if (EquippedItem)
            EquippedItem.SetActive(false);
        EquippedItem = null;
    }

    public void Attack()
    {
        Anim.SetTrigger("Attack");
        //플레이어 입력과 이동을 비활성화
        LockControl();
    }

    //플레이어가 장애물을 제거하는 것이므로 사냥꾼 쪽에서 제거 코드를 실행
    void RemoveObstacle(GameObject _obstacle)
    {
        Destroy(_obstacle);
        //파티클 재생 가능
        //파티클 위치는 제거 대상의 위치
    }

    void LockControl()
    {
        Movement.enabled = false;
        Input.enabled = false;
    }
    void UnlockControl()
    {
        Movement.enabled = true;
        Input.enabled = true;
    }
}
