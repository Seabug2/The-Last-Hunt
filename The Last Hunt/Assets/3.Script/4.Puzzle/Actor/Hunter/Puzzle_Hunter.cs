using UnityEngine;

public class Puzzle_Hunter : Puzzle_Actor
{

    public Animator Anim { get; private set; }

    public GameObject EquippedItem { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Anim = GetComponent<Animator>();
        FindTargetTransform(this.transform, "jointItemR");
    }

    private void Start()
    {
        UnequipItem();
    }

    [SerializeField]
    Transform hand;
    public Transform EquippedHand => hand;

    public void FindTargetTransform(Transform _root, string _name)
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

    public void UnequipItem()
    {
        if (EquippedItem)
            EquippedItem.SetActive(false);
        EquippedItem = null;
    }

    public void PlayUsingItem()
    {
        Anim.SetTrigger("Attack");
    }

    public void RemoveObstacle()
    {

    }


    public void GameOver()
    {
        GetComponent<Puzzle_Hunter_Input>().enabled = false;
        GetComponent<Puzzle_Hunter_Movement>().enabled = false;
        GetComponent<Puzzle_Hunter_TileAction>().enabled = false;

        Puzzle_GameManager.instance.GameOverEvent?.Invoke();
    }

}
