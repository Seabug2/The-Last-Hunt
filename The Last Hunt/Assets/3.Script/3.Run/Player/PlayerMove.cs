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
        //코루틴으로 할지 고민
        StartMoveCo();
    }


    private void FixedUpdate()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        transform.Translate(transform.forward * forwardSpeed * Time.deltaTime);

        //나중에 코너 때문에 transform.Forward를 조건에 맞게 바꿔야함.
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
            Debug.Log("점프 입력이 되긴했는데요");
            if (jumpCount > 0)
            {
                Debug.Log("점프 코르틴 안으로 들어오긴 했는데요");
                StartCoroutine("Jump_co");
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("슬라이드 입력이 되긴했는데요");

            if (slideCount>0)
                Debug.Log("슬라이드 코르틴 안으로 들어오긴 했는데요");
            {
                StartCoroutine("Slide_co");
            }
        
        }
    }
   

    private IEnumerator Jump_co()
    {
        Debug.Log("아 점프 들어오긴 했는데요");
        Player_rig.AddForce(new Vector3(0, JumpForce, 0));
        player_ani.SetBool("isJump", true);
        player_ani.SetBool("isRun", false);

        isJump = true;
        jumpCount = 0;
        yield return new WaitForSeconds(0.5f);
        Debug.Log("아 점프 넘어오긴 했는데요");
        player_ani.SetBool("isJump", false);
        player_ani.SetBool("isRun", true);
        jumpCount = 1;
        isJump = false;
        Debug.Log("아 점프 끝까지 왔는데요");
        
    
    }
    private IEnumerator Slide_co()
    {
        
        Debug.Log("아 슬라이드 들어오긴 했는데요");
       
            player_ani.SetBool("isSlide", true);
            player_ani.SetBool("isRun", false);
            isSlide = true;
            player_c.height = 1;
            slideCount=0;
            yield return new WaitForSeconds(1f);
            Debug.Log("아 슬라이드 넘어오긴 했는데요");
            player_ani.SetBool("isSlide", false);
            player_ani.SetBool("isRun", true);
            player_c.height = 2;
            slideCount = 1;
            isSlide = false;
            Debug.Log("아 슬라이드 끝까지 왔는데요");
        
       
    }
}
