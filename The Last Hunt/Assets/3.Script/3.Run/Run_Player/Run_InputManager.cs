using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_InputManager : MonoBehaviour
{
    public static Run_InputManager instance = null;

    private Animator player_ani;
    public LayerMask originLayer;

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

    public LayerMask turnTileLayer;

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
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Ÿ���Էµ�");
            // �ٴ� �˻縦 �ǽ�
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, Mathf.Infinity, turnTileLayer))
            {

                Debug.Log("Ÿ�� ù°���� Ŭ����");
                if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow))
                {
                    //��ȸ��
                    transform.Rotate(new Vector3(0f, 90f, 0f));
                    hit.collider.gameObject.layer = 4;
                   
                    Debug.Log("Ÿ�� ������ ���� Ŭ����");
                    return;
                }
                
                else if(Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    //��ȸ��
                    transform.Rotate(new Vector3(0f, -90f, 0f));
                    hit.collider.gameObject.layer = 4;
                    
                    Debug.Log("Ÿ�� ���� ���� Ŭ����");
                    return;
                }
                
            }
            
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            move.PlayerMove();
        }


        
    }
}
