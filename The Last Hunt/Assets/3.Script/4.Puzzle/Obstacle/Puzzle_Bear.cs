using UnityEngine;

public class Puzzle_Bear : MonoBehaviour
{
    GameObject target = null;

    //������ �����ϸ� �÷��̾� ���� �ߴ�
    void Attack()
    {
        //������ �ߵ��Ǹ� �÷��̾� ������ �ߴ�
        Puzzle_GameManager.instance.EndGame?.Invoke();
        GetComponent<Animator>().SetTrigger("Attack");
    }

    public void RemoveTarget()
    {
        if (target.TryGetComponent(out Puzzle_Horse horse))
        {
            Puzzle_GameManager.instance.GameOver_Horse(horse);
        }
        else if (target.TryGetComponent(out Puzzle_Hunter_TileAction hunter))
        {
            Puzzle_GameManager.instance.GameOver_Hunter(hunter);
        }

        GetComponent<AudioSource>().Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        //������ IsGameOver ���¶�� ������ ���� �ʴ´�.
        if (Puzzle_GameManager.instance.IsGameOver) return;

        target = other.gameObject;

        //������ ���� ��ü�� �ٶ󺻴�
        Vector3 dir = other.transform.position;
        transform.LookAt(new Vector3(dir.x, 0, dir.z), Vector3.up);

        if (other.TryGetComponent(out Puzzle_Horse horse))
        {
            //���� ����
            Puzzle_GameManager.instance.VCamFollowHorse();
            Attack();
        }
        else if (other.TryGetComponent(out Puzzle_Hunter hunter))
        {
            //��� �����ϰ� ���� ���� ���
            if (!hunter.EquippedItem)
            {
                //�÷��̾ ����
                Puzzle_GameManager.instance.LookAtHunter();
                Attack();
            }
        }
    }
}
