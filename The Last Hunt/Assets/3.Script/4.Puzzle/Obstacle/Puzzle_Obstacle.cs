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
                if (_hunter.EquippedItem.name.Equals(RequiredName))
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
        else if (other.TryGetComponent(out Puzzle_Horse_TileAction _horse))
        {
            MeetHorse(_horse);
        }
    }

    //�÷��̾�� ������ ��
    //�÷��̾ �˸��� ��� ������ ���
    protected virtual void RemoveSelf(Puzzle_Hunter _hunter)
    {
        _hunter.SetTrigger("Attack");
        Destroy(this.gameObject);

        ////ĳ���Ͱ� ������Ʈ�� �ٶ󺸰� �մϴ�.
        //print("��ֹ��� �����մϴ�.");
        ////��� ĳ������ �̵��� �÷��̾��� �Է��� �����ϴ�.
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
    protected virtual void MeetHorse(Puzzle_Horse_TileAction _horse)
    {
        print("���� �߸��� ��� �Խ��ϴ�. (���ӿ���)");
    }
}
