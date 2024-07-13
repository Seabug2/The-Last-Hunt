using UnityEngine;

public class Puzzle_Hunter_Tool :MonoBehaviour
{
    public bool currentItem {
        get; private set;
    }

    /// <summary>
    /// 0 : ���
    /// 1 : ����
    /// </summary>
    [SerializeField, Header("0 : ��� / 1 : ����"), Space(10)]
    GameObject[] Items;

    private void Start()
    {
        UnequipItem();
    }

    public void EquipItem(ItemType itemType)
    {
        Items[(int)itemType].SetActive(true);
        currentItem = true;
    }

    public void UnequipItem()
    {
        foreach (GameObject item in Items)
        {
            item.SetActive(false);
        }
        currentItem = false;
    }
}
