using UnityEngine;

public class Puzzle_Hunter : MonoBehaviour
{
    [HideInInspector]
    public bool isAlive = true;

    public Animator Anim { get; private set; }
    public Puzzle_TileChecker TileChecker { get; private set; }
    public Puzzle_Hunter_Input Input { get; private set; }

    /// <summary>
    /// ���� ���� ���� ������
    /// </summary>
    public GameObject EquippedItem { get; private set; }


    void Awake()
    {
        Anim = GetComponent<Animator>();
        TileChecker = GetComponent<Puzzle_TileChecker>();
        this.Input = GetComponent<Puzzle_Hunter_Input>();
        //FindTargetTransform(this.transform, "jointItemR");
    }

    private void Start()
    {
        UnequipItem();
        //Puzzle_GameManager.instance.GameOverEvent += () => {};
    }

    public void Attack()
    {
        this.Input.enabled = false;
        Anim.SetTrigger("Attack");
        //�÷��̾� �Է°� �̵��� ��Ȱ��ȭ
    }

    public void EndMotion()
    {
        //�÷��̾� �Է��� �ٽ� Ȱ��ȭ
        this.Input.enabled = true;
    }

    //�׾ ���ӿ���
    public void Falling()
    {
        Anim.SetTrigger("Falling");
    }

    [SerializeField]
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

    //�ִϸ��̼� Ű �̺�Ʈ�� ����
    public void UnequipItem()
    {
        if (EquippedItem)
            EquippedItem.SetActive(false);
        EquippedItem = null;
    }

    //�÷��̾ ��ֹ��� �����ϴ� ���̹Ƿ� ��ɲ� �ʿ��� ���� �ڵ带 ����
    public void RemoveObstacle(GameObject _obstacle)
    {
        Destroy(_obstacle);
        //��ƼŬ ��� ����
        //��ƼŬ ��ġ�� ���� ����� ��ġ
    }

    public void GameOver()
    {
        isAlive = false;
        Puzzle_GameManager.instance.GameOverEvent?.Invoke();
    }
}
