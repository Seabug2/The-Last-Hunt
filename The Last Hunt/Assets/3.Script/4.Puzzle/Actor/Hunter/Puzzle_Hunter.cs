using UnityEngine;

public class Puzzle_Hunter : MonoBehaviour
{
    //아이템을 사용하고 공격하는 모션은 Hunter 클래스에서 관리
    public Puzzle_Hunter_Movement Movement { get; private set; }
    public Puzzle_Hunter_Input Input { get; private set; }
    public Animator Anim { get; private set; }

    void Awake()
    {
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

        Freeze();

        Puzzle_GameManager.instance.GameStartEvent.AddListener(Unfreeze);

        Puzzle_GameManager.instance.EndGame.AddListener(() =>
        {
            UnequipItem();
            Freeze();
        });

        Puzzle_GameManager.instance.GameClearEvent.AddListener(() =>
        {
            Anim.SetTrigger("Clear");
        });

        GetComponent<Puzzle_Hunter_TileAction>().FallingEvent.AddListener(() =>
        {
            Anim.SetTrigger("Falling");
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        //장애물과 부딪혔을 때
        if (other.CompareTag("Obstacle"))
        {
            //아이템을 장착 중이고
            if (EquippedItem)
            {
                //아이템이 알맞은 아이템인 경우
                if (EquippedItem.name == other.GetComponent<Puzzle_Obstacle>().RequiredName
                    || other.TryGetComponent(out Puzzle_Wolf _))
                {
                    Destroy(other.GetComponent<Collider>());
                    target = other.gameObject;
                    Attack();
                    Vector3 dir = other.transform.position;
                    transform.LookAt(new Vector3(dir.x, 0, dir.z), Vector3.up);
                    return;
                }
            }
        }
        //아이템과 충돌시
        if (other.CompareTag("Item"))
        {
            if (EquippedItem)
            {
                Puzzle_GameManager.instance.ShowMessage("현재 장착한 장비가 남아있습니다", .25f);
            }
            else
            {
                EquipItem(other.name);
                Destroy(other.gameObject);
            }
        }
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
        Freeze();
    }

    GameObject target = null;
    //플레이어가 장애물을 제거하는 것이므로 사냥꾼 쪽에서 제거 코드를 실행
    public void RemoveObstacle()
    {
        if (target == null)
        {
            return;
        }

        if (target.TryGetComponent(out Puzzle_Wolf wolf))
        {
            wolf.GetComponent<Animator>().SetTrigger("Dead");
        }
        else
        {
            Destroy(target);
        }

        UnequipItem();
        target = null;
    }

    /// <summary>
    /// 게임 초기화
    /// </summary>
    public void Freeze()
    {
        Movement.enabled = false;
        Input.enabled = false;
        Anim.SetBool("isMoving", false);
    }

    /// <summary>
    /// 게임 시작시, 공격을 마쳤을 때 실행
    /// </summary>
    public void Unfreeze()
    {
        Movement.enabled = true;
        Input.enabled = true;
    }
}
