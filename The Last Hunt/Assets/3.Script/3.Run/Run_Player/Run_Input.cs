using UnityEngine;

public class Run_Input : MonoBehaviour
{
    private void Awake()
    {
        move = GetComponent<Run_PlayerMove>();
        voice = GetComponent<Run_HunterVoice>();
        tileChecker = GetComponent<Run_TileChecker>();
    }

    Run_PlayerMove move;
    Run_TileChecker tileChecker;
    Run_HunterVoice voice;

    private void Update()
    {
        //만약 캐릭터가 점프 중이거나 슬라이딩 중이면 조작을 할 수 없다.
        if (move.isJumping || move.isSliding) return;

        //만약 스페이스바를 눌렀으면
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            voice.PlayAudio("기합소리");
            //캐릭터가 점프를 한다.
            move.Jumping();
            return;
        }

        //만약 s 키를 눌렀으면
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            voice.PlayAudio("기합소리");
            //슬라이딩을 실행
            move.Sliding();
            return;
        }

        //좌우 방향키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
            //바닥 검사를 실시
            if (Physics.Raycast(ray, out RaycastHit hit, 2f, 1 << LayerMask.NameToLayer("Tile")))
            {
                GameObject go = hit.collider.gameObject;
                if (!go.CompareTag("Turn")) return;

                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    move.TurnRight();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    move.TurnLeft();
                }

                go.layer = 0;
                return;
            }
        }
    }
}
