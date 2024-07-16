using UnityEngine;

public class Puzzle_Hunter_Input : MonoBehaviour
{
    Puzzle_Hunter_Movement movement;
    Puzzle_Hunter_TileAction carrying;
    Animator anim;

    private void Awake()
    {
        carrying = GetComponent<Puzzle_Hunter_TileAction>();
        movement = GetComponent<Puzzle_Hunter_Movement>();
        anim = GetComponent<Animator>();
    }

    public float dead = 1f;
    Vector3 dir;

    private void Update()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            anim.SetBool("isMoving", true);
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 norDir = new Vector3(x, 0, z).normalized;
            dir = new Vector3(norDir.x * Mathf.Abs(x), 0, norDir.z * Mathf.Abs(z));
        }
        else
        {
            anim.SetBool("isMoving", false);
            dir = Vector3.Lerp(dir, Vector3.zero, Time.deltaTime * dead);
        }

        movement.dir = dir;

        //스페이스바를 눌렀을 때 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            carrying.CarryingAction();
        }
    }
}