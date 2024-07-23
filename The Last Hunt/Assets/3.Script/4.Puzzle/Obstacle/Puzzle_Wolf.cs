using UnityEngine;

public class Puzzle_Wolf : Puzzle_Obstacle
{
    GameObject target = null;

    //공격을 시작하면 플레이어 조작 중단
    void Attack(GameObject target)
    {
        this.target = target;
        //게임이 IsGameOver 상태라면 공격을 하지 않는다.
        if (Puzzle_GameManager.instance.IsGameOver) return;

        //공격이 발동되면 플레이어 조작을 중단
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
        //영역에 닿은 객체를 바라본다
        Vector3 dir = other.transform.position;
        transform.LookAt(new Vector3(dir.x, 0, dir.z), Vector3.up);

        if (other.TryGetComponent(out Puzzle_Horse _))
        {
            //말을 공격
            Puzzle_GameManager.instance.VCamFollowHorse();
            Attack(other.gameObject);
        }
        else if (other.TryGetComponent(out Puzzle_Hunter hunter))
        {
            //장비를 장착하고 있지 않은 경우
            if (!hunter.EquippedItem)
            {
                //플레이어를 공격
                Puzzle_GameManager.instance.LookAtHunter();
                Attack(other.gameObject);
            }
        }
    }

}
