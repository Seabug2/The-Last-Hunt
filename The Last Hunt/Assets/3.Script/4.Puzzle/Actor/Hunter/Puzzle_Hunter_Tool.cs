using UnityEngine;

public class Puzzle_Hunter_Tool
{
    public Puzzle_Item currentItem = null;

    /// <summary>
    /// 0 : °î±ªÀÌ
    /// 1 : µµ³¢
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
