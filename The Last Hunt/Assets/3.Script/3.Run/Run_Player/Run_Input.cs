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
        //���� ĳ���Ͱ� ���� ���̰ų� �����̵� ���̸� ������ �� �� ����.
        if (move.isJumping || move.isSliding) return;

        //���� �����̽��ٸ� ��������
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            voice.PlayAudio("���ռҸ�");
            //ĳ���Ͱ� ������ �Ѵ�.
            move.Jumping();
            return;
        }

        //���� s Ű�� ��������
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            voice.PlayAudio("���ռҸ�");
            //�����̵��� ����
            move.Sliding();
            return;
        }

        //�¿� ����Ű�� ������ ��
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
            //�ٴ� �˻縦 �ǽ�
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
