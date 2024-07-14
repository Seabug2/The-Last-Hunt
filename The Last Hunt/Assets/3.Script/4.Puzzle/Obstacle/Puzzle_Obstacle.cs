using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Obstacle : MonoBehaviour
{
    [SerializeField]
    string RequiredName;

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);

        //���?
        if (other.TryGetComponent(out Puzzle_Hunter _hunter))
        {
            //��� �־�?
            if (_hunter.EquippedItem)
            {
                if (IsCorrectItem(_hunter.EquippedItem.name))
                {
                    //���� ��ȸ���Դϴ�. ���� ����
                    _hunter.UnequipItem();
                    //�� ������Ʈ�� �����մϴ�.
                    RemoveSelf(_hunter);
                    return;
                }
            }

            Mismatch(_hunter);
            return;
        }

        //��?
        else if (other.TryGetComponent(out Puzzle_Horse _horse))
        {
            MeetHorse(_horse);
        }
    }

    protected virtual bool IsCorrectItem(string _itemName)
    {
        return _itemName.Equals(RequiredName);
    }

    //�÷��̾�� ������ ��
    //�÷��̾ �˸��� ��� ������ ���
    protected virtual void RemoveSelf(Puzzle_Hunter _hunter)
    {
        _hunter.Attack();
        //Destroy(this.gameObject);

        ////ĳ���Ͱ� ������Ʈ�� �ٶ󺸰� �մϴ�.
        //Vector3 dir = (transform.position - _hunter.transform.position).normalized;
        //_hunter.transform.forward = new Vector3(dir.x, transform.position.y, dir.z);
        //_hunter.Anim.SetTrigger("Attack");
    }

    //����) �÷��̾ ��� �߸� ������ ���
    protected virtual void Mismatch(Puzzle_Hunter _hunter)
    {
        print("�߸��� ��� �����ϰ� �ֽ��ϴ�. (���ӿ���)");
    }

    //���� ������ ��
    protected virtual void MeetHorse(Puzzle_Horse _horse)
    {
        print("���� �߸��� ��� �Խ��ϴ�. (���ӿ���)");
    }
}
