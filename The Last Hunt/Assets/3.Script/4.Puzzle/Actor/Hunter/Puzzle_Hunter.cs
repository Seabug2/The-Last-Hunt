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
        //���� ���۽� hand�� �غ��� �����۵��� ��Ȱ��ȭ�Ͽ� ������ �ʰ� �մϴ�.
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
    /// ���� ���� ���� ������
    /// </summary>
    public GameObject EquippedItem { get; private set; }

    [SerializeField, Header("���⸦ ������ �ǹ� ��ġ")]
    Transform hand;
    public Transform EquippedHand => hand;

    //DFS ����Լ�
    void FindTargetTransform(Transform _root, string _name)
    {
        foreach (Transform t in _root)
        {
            if (hand != null) return;
            else if (t.name.Equals(_name))
            {
                hand = t;
                print(_name + "�� ã�ҽ��ϴ�!");

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
    /// ������ �̸��� ��� ã�� �����մϴ�.
    /// </summary>
    /// <param name="_PickUpItemName"></param>
    public void EquipItem(string _PickUpItemName)
    {
        GameObject pickUpItem = hand.Find(_PickUpItemName).gameObject;
        pickUpItem.SetActive(true);
        EquippedItem = pickUpItem;
    }
    /// <summary>
    /// �������� ����ϰ� ���� �����մϴ�
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
        //�÷��̾� �Է°� �̵��� ��Ȱ��ȭ
        LockControl();
    }

    //�÷��̾ ��ֹ��� �����ϴ� ���̹Ƿ� ��ɲ� �ʿ��� ���� �ڵ带 ����
    void RemoveObstacle(GameObject _obstacle)
    {
        Destroy(_obstacle);
        //��ƼŬ ��� ����
        //��ƼŬ ��ġ�� ���� ����� ��ġ
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
