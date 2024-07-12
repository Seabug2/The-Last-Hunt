using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public enum ItemType
    {
        //�߼��� ���� �� �ְ� => ����, ���
        //������ �� �� �ְ� => ����
        //������ �� �� �ְ� => ���
        Pick,
        Axe
    }
public class Puzzle_Item : MonoBehaviour
{
    public ItemType itemType;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Puzzle_Hunter_Tool tool))
        {
            if (tool.currentItem == null)
            {
                tool.currentItem = this;
                //��Ȱ��ȭ
                enabled = false;
                //UI ȭ�鿡 ������ ����
            }
        }
    }
}
