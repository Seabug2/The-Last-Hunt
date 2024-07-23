using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_PlayerMove : MonoBehaviour
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float JumpForce;
    //public int jumpCount =1, slideCount = 1;

    public bool isJump = false;
    public bool isSlide = false;

    private Rigidbody Player_rig;
    private CapsuleCollider player_c;
    private Animator player_ani;

    private void Start()
    {
        Player_rig = GetComponent<Rigidbody>();
        player_c = GetComponent<CapsuleCollider>();
        player_ani = GetComponent<Animator>();
    }
    private void Update()
    {
        //StartMove();
        //Jump();
        //Sliding();
        //코루틴으로 할지 고민
    }


    private void FixedUpdate()
    {
        if (!isJump)
            MoveForward();
    }

    private void MoveForward()
    {
        Player_rig.velocity = transform.forward * Time.fixedDeltaTime * forwardSpeed;

        //transform.Translate(transform.forward * forwardSpeed * Time.deltaTime);
        //Vector3 forwardMove = transform.forward * forwardSpeed * Time.fixedDeltaTime;
        //Player_rig.position += forwardMove;
        //Player_rig.MovePosition(Player_rig.position + forwardMove);


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


    public void PlayerJump()
    {
        Debug.Log("PlayerJump 진입");
        if (!isJump && !isSlide)
        {
            isJump = true;
            Debug.Log("isJump는 True입니다.");
            player_ani.SetBool("isJump", true);
            player_ani.SetBool("isRun", false);
            Debug.Log("isJump_animation은 True입니다.");
            Player_rig.AddForce(new Vector3(0, JumpForce, 0), ForceMode.VelocityChange);
            
        }
        
    }
    public void EndJump()//점프 animation KeyFrame에 추가할 메서드
    {
        isJump = false;
        player_ani.SetBool("isJump", false);
        player_ani.SetBool("isRun", true); 
        Debug.Log("isJump는 false입니다.");
    }

    public void PlayerSlide()
    {
        isSlide = true;
        player_ani.SetBool("isSlide", true);
        player_ani.SetBool("isRun", false);
        Debug.Log("isSlide는 True입니다.");
    }
    public void EndSlide()//슬라이드 animation Keyframe에 추가할 메서드
    {
        isSlide = false;
        player_ani.SetBool("isSlide", false);
        player_ani.SetBool("isRun", true);
        Debug.Log("isSlide는 false입니다.");

    }
    public void PlayerRun()
    {
        isJump = false;
        isSlide = false;
    }

    // private IEnumerator Jump_co()
    // {
    //     Debug.Log("아 점프 들어오긴 했는데요");
    //     Player_rig.AddForce(new Vector3(0, JumpForce, 0));
    //     player_ani.SetBool("isJump", true);
    //     player_ani.SetBool("isRun", false);
    //
    //     isJump = true;
    //     jumpCount = 0;
    //     yield return new WaitForSeconds(0.5f);
    //     Debug.Log("아 점프 넘어오긴 했는데요");
    //     player_ani.SetBool("isJump", false);
    //     player_ani.SetBool("isRun", true);
    //     jumpCount = 1;
    //     isJump = false;
    //     Debug.Log("아 점프 끝까지 왔는데요");
    //     
    // 
    // }
    // private IEnumerator Slide_co()
    // {
    //     
    //     Debug.Log("아 슬라이드 들어오긴 했는데요");
    //    
    //         player_ani.SetBool("isSlide", true);
    //         player_ani.SetBool("isRun", false);
    //         isSlide = true;
    //         player_c.height = 1;
    //         slideCount=0;
    //         yield return new WaitForSeconds(1f);
    //         Debug.Log("아 슬라이드 넘어오긴 했는데요");
    //         player_ani.SetBool("isSlide", false);
    //         player_ani.SetBool("isRun", true);
    //         player_c.height = 2;
    //         slideCount = 1;
    //         isSlide = false;
    //         Debug.Log("아 슬라이드 끝까지 왔는데요");
    //     
    //    
    // }


}
