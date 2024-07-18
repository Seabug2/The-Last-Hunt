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
                Puzzle_GameManager.instance.SendMessage("���� ���� ���� �������� �ֽ��ϴ�");
            }
        }
    }
}
