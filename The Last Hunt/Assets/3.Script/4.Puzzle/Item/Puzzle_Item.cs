using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    //�߼��� ���� �� �ְ� => ����, ���
    //������ �� �� �ְ� => ����
    //������ �� �� �ְ� => ���
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
                //UI ȭ�鿡 ������ ����

                Destroy(gameObject);
            }
            else
            {
                print("���� ���� ���� �������� �ֽ��ϴ�");
            }
        }
    }
}
