using UnityEngine;

public class Puzzle_Hunter : MonoBehaviour
{
    [HideInInspector]
    public bool isAlive = true;

    public Animator Anim { get; private set; }
    public Puzzle_TileChecker TileChecker { get; private set; }

    /// <summary>
    /// 현재 장착 중인 아이템
    /// </summary>
    public GameObject EquippedItem { get; private set; }


    void Awake()
    {
        Anim = GetComponent<Animator>();
        TileChecker = GetComponent<Puzzle_TileChecker>();
        FindTargetTransform(this.transform, "jointItemR");
    }

    private void Start()
    {
        UnequipItem();

        Puzzle_GameManager.instance.GameOverEvent.AddListener(() => {

        });
    }

    public void Attack()
    {
        Anim.SetTrigger("Attack");
        //플레이어 입력과 이동을 비활성화
    }

    public void EndMotion()
    {
        //플레이어 입력과 이동을 다시 활성화
    }

    //죽어도 게임오버
    public void Falling()
    {
        Anim.SetTrigger("Attack");
    }

    void CanNotControl()
    {

    }

    [SerializeField]
    Transform hand;
    public Transform EquippedHand => hand;

    void FindTargetTransform(Transform _root, string _name)
    {
        foreach (Transform t in _root)
        {
            if (hand != null) return;
            else if (t.name.Equals(_name))
            {
                hand = t;
                print(_name + "를 찾았습니다!");

                foreach(Transform child in t)
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

    public void EquipItem(string _PickUpItemName)
    {
        GameObject pickUpItem = hand.Find(_PickUpItemName).gameObject;
        pickUpItem.SetActive(true);
        EquippedItem = pickUpItem;
    }

    //애니메이션 키 이벤트로 실행
    public void UnequipItem()
    {
        if (EquippedItem)
            EquippedItem.SetActive(false);
        EquippedItem = null;
    }

    //플레이어가 장애물을 제거하는 것이므로 사냥꾼 쪽에서 제거 코드를 실행
    public void RemoveObstacle(GameObject _obstacle)
    {
        Destroy(_obstacle);
        //파티클 재생 가능
        //파티클 위치는 제거 대상의 위치
    }

    public void GameOver()
    {
        isAlive = false;

        GetComponent<Puzzle_Hunter_Input>().enabled = false;
        GetComponent<Puzzle_Hunter_Movement>().enabled = false;
        GetComponent<Puzzle_Hunter_TileAction>().enabled = false;

        Puzzle_GameManager.instance.GameOverEvent?.Invoke();
    }
}
