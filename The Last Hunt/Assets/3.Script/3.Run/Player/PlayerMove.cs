using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float JumpForce;
    private int jumpCount =1, slideCount = 1;
    private bool isJump = false;
    private bool isSlide = false;
    private CapsuleCollider player_c;

    private Animator player_ani;

    private Rigidbody Player_rig;

    private void Start()
    {
        player_c = GetComponent<CapsuleCollider>();
        player_ani = GetComponent<Animator>();
        Player_rig = GetComponent<Rigidbody>();
        Player_rig.velocity = Vector3.zero;
        player_ani.SetBool("isRun", true);
    }

    private void Update()
    {
        //Jump();
        //Sliding();
        //�ڷ�ƾ���� ���� ���
        StartMoveCo();
    }


    private void FixedUpdate()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        transform.Translate(transform.forward * forwardSpeed * Time.deltaTime);

        //���߿� �ڳ� ������ transform.Forward�� ���ǿ� �°� �ٲ����.
    }
    //private void Jump()
    //{
    //    if (jumpCount > 0)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            player_ani.SetBool("isJump", true);
    //            player_ani.SetBool("isRun", false);
    //
    //            Player_rig.AddForce(new Vector3(0, JumpForce, 0));
    //            isJump = true;
    //            if (player_ani.GetBool("isJump"))
    //            {
    //                jumpCount--;
    //            }
    //
    //        }
    //    }
    //    
    //}

    //private void Sliding()
    //{
    //    if(slideCount>0)
    //    {
    //        if(Input.GetKeyDown(KeyCode.S))
    //        {
    //            player_ani.SetBool("isSlide", true);
    //            player_ani.SetBool("isRun", false);
    //
    //            player_c.height = 1;
    //            slideCount--;
    //        }
    //
    //
    //    }
    //}

    private void StartMoveCo()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("���� �Է��� �Ǳ��ߴµ���");
            if (jumpCount > 0)
            {
                Debug.Log("���� �ڸ�ƾ ������ ������ �ߴµ���");
                StartCoroutine("Jump_co");
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("�����̵� �Է��� �Ǳ��ߴµ���");

            if (slideCount>0)
                Debug.Log("�����̵� �ڸ�ƾ ������ ������ �ߴµ���");
            {
                StartCoroutine("Slide_co");
            }
        
        }
    }
   

    private IEnumerator Jump_co()
    {
        Debug.Log("�� ���� ������ �ߴµ���");
        Player_rig.AddForce(new Vector3(0, JumpForce, 0));
        player_ani.SetBool("isJump", true);
        player_ani.SetBool("isRun", false);

        isJump = true;
        jumpCount = 0;
        yield return new WaitForSeconds(0.5f);
        Debug.Log("�� ���� �Ѿ���� �ߴµ���");
        player_ani.SetBool("isJump", false);
        player_ani.SetBool("isRun", true);
        jumpCount = 1;
        isJump = false;
        Debug.Log("�� ���� ������ �Դµ���");
        
    
    }
    private IEnumerator Slide_co()
    {
        
        Debug.Log("�� �����̵� ������ �ߴµ���");
       
            player_ani.SetBool("isSlide", true);
            player_ani.SetBool("isRun", false);
            isSlide = true;
            player_c.height = 1;
            slideCount=0;
            yield return new WaitForSeconds(1f);
            Debug.Log("�� �����̵� �Ѿ���� �ߴµ���");
            player_ani.SetBool("isSlide", false);
            player_ani.SetBool("isRun", true);
            player_c.height = 2;
            slideCount = 1;
            isSlide = false;
            Debug.Log("�� �����̵� ������ �Դµ���");
        
       
    }
}
