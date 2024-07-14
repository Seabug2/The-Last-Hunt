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

        //사람?
        if (other.TryGetComponent(out Puzzle_Hunter _hunter))
        {
            //장비 있어?
            if (_hunter.EquippedItem)
            {
                if (IsCorrectItem(_hunter.EquippedItem.name))
                {
                    //장비는 일회용입니다. 착용 해제
                    _hunter.UnequipItem();
                    //이 오브젝트를 제거합니다.
                    RemoveSelf(_hunter);
                    return;
                }
            }

            Mismatch(_hunter);
            return;
        }

        //말?
        else if (other.TryGetComponent(out Puzzle_Horse _horse))
        {
            MeetHorse(_horse);
        }
    }

    protected virtual bool IsCorrectItem(string _itemName)
    {
        return _itemName.Equals(RequiredName);
    }

    //플레이어와 만났을 때
    //플레이어가 알맞은 장비를 장착한 경우
    protected virtual void RemoveSelf(Puzzle_Hunter _hunter)
    {
        _hunter.Attack();
        //Destroy(this.gameObject);

        ////캐릭터가 오브젝트를 바라보게 합니다.
        //Vector3 dir = (transform.position - _hunter.transform.position).normalized;
        //_hunter.transform.forward = new Vector3(dir.x, transform.position.y, dir.z);
        //_hunter.Anim.SetTrigger("Attack");
    }

    //실행) 플레이어가 장비를 잘못 장착한 경우
    protected virtual void Mismatch(Puzzle_Hunter _hunter)
    {
        print("잘못된 장비를 장착하고 있습니다. (게임오버)");
    }

    //말을 만났을 때
    protected virtual void MeetHorse(Puzzle_Horse _horse)
    {
        print("말이 잘못된 길로 왔습니다. (게임오버)");
    }
}
