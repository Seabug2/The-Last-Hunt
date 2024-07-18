using UnityEngine;

public class Puzzle_Hunter : MonoBehaviour
{
    //�������� ����ϰ� �����ϴ� ����� Hunter Ŭ�������� ����
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
        //���� ���۽� hand�� �غ��� �����۵��� ��Ȱ��ȭ�Ͽ� ������ �ʰ� �մϴ�.
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
        //��ֹ��� �ε����� ��
        if (other.CompareTag("Obstacle"))
        {
            //�������� ���� ���̰�
            if (EquippedItem)
            {
                //�������� �˸��� �������� ���
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
        //�����۰� �浹��
        if (other.CompareTag("Item"))
        {
            if (EquippedItem)
            {
                Puzzle_GameManager.instance.ShowMessage("���� ������ ��� �����ֽ��ϴ�", .25f);
            }
            else
            {
                EquipItem(other.name);
                Destroy(other.gameObject);
            }
        }
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
        Freeze();
    }

    GameObject target = null;
    //�÷��̾ ��ֹ��� �����ϴ� ���̹Ƿ� ��ɲ� �ʿ��� ���� �ڵ带 ����
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
    /// ���� �ʱ�ȭ
    /// </summary>
    public void Freeze()
    {
        Movement.enabled = false;
        Input.enabled = false;
        Anim.SetBool("isMoving", false);
    }

    /// <summary>
    /// ���� ���۽�, ������ ������ �� ����
    /// </summary>
    public void Unfreeze()
    {
        Movement.enabled = true;
        Input.enabled = true;
    }
}
