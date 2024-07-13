using UnityEngine;

public class Puzzle_Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Puzzle_Hunter tool))
        {
            if (!tool.EquippedItem)
            {
                tool.EquipItem(name);
                Destroy(gameObject);
            }
            else
            {
                print("현재 장착 중인 아이템이 있습니다");
            }
        }
    }
}
