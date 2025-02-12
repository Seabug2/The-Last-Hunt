using UnityEngine;

public class Puzzle_Bear : MonoBehaviour
{
    GameObject target = null;

    //공격을 시작하면 플레이어 조작 중단
    void Attack()
    {
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
    }

    private void OnTriggerEnter(Collider other)
    {
        //게임이 IsGameOver 상태라면 공격을 하지 않는다.
        if (Puzzle_GameManager.instance.IsGameOver) return;

        target = other.gameObject;

        //영역에 닿은 객체를 바라본다
        Vector3 dir = other.transform.position;
        transform.LookAt(new Vector3(dir.x, 0, dir.z), Vector3.up);

        if (other.TryGetComponent(out Puzzle_Horse _))
        {
            //말을 공격
            Puzzle_GameManager.instance.VCamFollowHorse();
            Attack();
        }
        else if (other.TryGetComponent(out Puzzle_Hunter hunter))
        {
            Puzzle_GameManager.instance.LookAtHunter();
            Attack();
        }
    }
}
