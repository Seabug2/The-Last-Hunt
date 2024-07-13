using UnityEngine;

public class Puzzle_Hunter_Tool
{
    public Puzzle_Item currentItem = null;

    /// <summary>
    /// 0 : ���
    /// 1 : ����
    /// </summary>
    GameObject[] Items;

    public void EquipItem(ItemType itemType)
    {
        Items[(int)itemType].SetActive(true);
    }

    public void UnequipItem()
    {
        foreach (GameObject item in Items)
        {
            item.SetActive(false);
        }
    }
}
