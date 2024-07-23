using UnityEngine;

public class Puzzle_Wolf : Puzzle_Obstacle
{
    GameObject target = null;

    //������ �����ϸ� �÷��̾� ���� �ߴ�
    void Attack(GameObject target)
    {
        this.target = target;
        //������ IsGameOver ���¶�� ������ ���� �ʴ´�.
        if (Puzzle_GameManager.instance.IsGameOver) return;

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
        //������ ���� ��ü�� �ٶ󺻴�
        Vector3 dir = other.transform.position;
        transform.LookAt(new Vector3(dir.x, 0, dir.z), Vector3.up);

        if (other.TryGetComponent(out Puzzle_Horse _))
        {
            //���� ����
            Puzzle_GameManager.instance.VCamFollowHorse();
            Attack(other.gameObject);
        }
        else if (other.TryGetComponent(out Puzzle_Hunter hunter))
        {
            //��� �����ϰ� ���� ���� ���
            if (!hunter.EquippedItem)
            {
                //�÷��̾ ����
                Puzzle_GameManager.instance.LookAtHunter();
                Attack(other.gameObject);
            }
        }
    }

}
