using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    //야수를 죽일 수 있고 => 도끼, 곡괭이
    //나무를 벨 수 있고 => 도끼
    //바위를 깰 수 있고 => 곡괭이
    Pick = 0,
    Axe = 1
}
public class Puzzle_Item : MonoBehaviour
{
    public ItemType itemType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Puzzle_Hunter_Tool tool))
        {
            if (!tool.currentItem)
            {
                tool.EquipItem(itemType);
                //UI 화면에 아이콘 띄우기

                Destroy(gameObject);
            }
            else
            {
                print("현재 장착 중인 아이템이 있습니다");
            }
        }
    }
}
