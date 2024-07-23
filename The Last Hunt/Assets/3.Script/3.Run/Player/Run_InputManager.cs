using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_InputManager : MonoBehaviour
{
    public static Run_InputManager instance = null;

    private Animator player_ani;

    //public int jumpCount = 1, slideCount = 1;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        move = GetComponent<Run_PlayerMove>();
        player_ani = GetComponent<Animator>();
    }

    Run_PlayerMove move;

    public LayerMask targetLayer;

    private void Update()
    {
        //���� ĳ���Ͱ� ���� ���̰ų� �����̵� ���̸� ������ �� �� ����.
        if (move.isJump || move.isSlide) return;

        //���� �����̽��ٸ� ��������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ĳ���Ͱ� ������ �Ѵ�.
            move.PlayerJump();
            Debug.Log("����");
            return;
        }

        //���� s Ű�� ��������
        if (Input.GetKeyDown(KeyCode.S))
        {
            //�����̵��� ����
            move.PlayerSlide();
            Debug.Log("�����̵�");
            return;
        }

        //if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        //{

        //�¿� �Է��� ���� ��
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            // �ٴ� �˻縦 �ǽ�
            if (Physics.Raycast(transform.position, Vector3.down, Mathf.Infinity, targetLayer))
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    //��ȸ��
                    transform.Rotate(new Vector3(0f, 90f, 0f));
                    return;
                }
                
                else if(Input.GetKeyDown(KeyCode.RightArrow))
                {
                    //��ȸ��
                    transform.Rotate(new Vector3(0f, -90f, 0f));
                    return;
                }
                
            }
        }


        //}
    }
}
