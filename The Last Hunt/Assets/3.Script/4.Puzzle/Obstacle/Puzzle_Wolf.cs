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

        Puzzle_GameManager.instance.EndGame?.Invoke();
        GetComponent<Animator>().SetTrigger("Attack");
        StartCoroutine(Puzzle_GameManager.instance.GameOver_Horse_co());
    }

    public void RemoveTarget()
    {
        if (target.TryGetComponent(out Puzzle_Horse horse))
        {

        }
        else if (target.TryGetComponent(out Puzzle_Hunter hunter))
        {
            hunter.Anim.SetTrigger("Dead");
        }
        target = null;
    }

    public void Dead()
    {
        //��ƼŬ

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //������ ���� ��ü�� �ٶ󺻴�
        Vector3 dir = other.transform.position;
        transform.LookAt(new Vector3(dir.x, 0, dir.z), Vector3.up);

        if (other.TryGetComponent(out Puzzle_Horse horse))
        {
            Attack(other.gameObject);
        }
        else if (other.TryGetComponent(out Puzzle_Hunter hunter))
        {
            //��� �����ϰ� ���� ���� ���
            if (!hunter.EquippedItem)
            {
                Attack(other.gameObject);
            }
        }
    }

}
