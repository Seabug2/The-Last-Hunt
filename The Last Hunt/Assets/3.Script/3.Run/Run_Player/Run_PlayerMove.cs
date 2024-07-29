using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_PlayerMove : MonoBehaviour
{
    [SerializeField] public float forwardSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float gravityForce;
    [SerializeField] private AudioSource bearAudio;
    [SerializeField] private AudioSource player_Audio;
    [SerializeField] private AudioClip dieClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip failClip;
    [SerializeField] GameObject bear;
    [SerializeField] GameObject player;



   
    //public int jumpCount =1, slideCount = 1;

    public bool isJump = false;
    public bool isSlide = false;
    private bool isDie = false;
    private bool isStart = false;

    Run_Result result;

    private float x;
    private Rigidbody player_rig;
    private CapsuleCollider player_c;
    private Animator player_ani;
    
    private Vector3 moveDir = Vector3.zero;

    private void Start()
    {
        
        bearAudio = bear.GetComponent<AudioSource>();
        player_rig = GetComponent<Rigidbody>();
        player_c = GetComponent<CapsuleCollider>();
        player_ani = GetComponent<Animator>();
        result = gameObject.GetComponent<Run_Result>();
        winClip = GetComponent<AudioClip>();
        failClip = GetComponent<AudioClip>();
        dieClip = GetComponent<AudioClip>();
    }
    private void Update()
    {
        
        //if (isDie) return;
        //    AddGravity();
    }


    private void FixedUpdate()
    {
        if (!isJump&&!isDie)
            MoveForward();
        
    }

    private void MoveForward()
    {
        if (isDie) return;
        player_rig.velocity = transform.forward * Time.fixedDeltaTime * forwardSpeed;

        //transform.Translate(transform.forward * forwardSpeed * Time.deltaTime);
        //Vector3 forwardMove = transform.forward * forwardSpeed * Time.fixedDeltaTime;
        //Player_rig.position += forwardMove;
        //Player_rig.MovePosition(Player_rig.position + forwardMove);


        //나중에 코너 때문에 transform.Forward를 조건에 맞게 바꿔야함.
    }
    //private void AddGravity()
    //{
    //    if (isDie) return;
    //    player_rig.AddForce(Vector3.down * gravityForce, ForceMode.Impulse);
    //}
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
        if (isDie) return;
        Debug.Log("PlayerJump 진입");
        if (!isJump && !isSlide)
        {
            isJump = true;
            Debug.Log("isJump는 True입니다.");
            player_ani.SetBool("isJump", true);
            player_ani.SetBool("isRun", false);
            Debug.Log("isJump_animation은 True입니다.");
            player_rig.AddForce(new Vector3(0, JumpForce, 0), ForceMode.VelocityChange);

        }

    }
    public void EndJump()//점프 animation KeyFrame에 추가할 메서드
    {
        if (isDie) return;
        isJump = false;
        player_ani.SetBool("isJump", false);
        player_ani.SetBool("isRun", true);
        Debug.Log("isJump는 false입니다.");
    }

    public void PlayerSlide()
    {
        if (isDie) return;
        isSlide = true;
        player_ani.SetBool("isSlide", true);
        player_ani.SetBool("isRun", false);
        player_c.height = 1;
        Debug.Log("isSlide는 True입니다.");
    }
    public void EndSlide()//슬라이드 animation Keyframe에 추가할 메서드
    {
        if (isDie) return;
        isSlide = false;
        player_ani.SetBool("isSlide", false);
        player_ani.SetBool("isRun", true);
        player_c.height = 2;
        Debug.Log("isSlide는 false입니다.");

    }
    public void PlayerRun()
    {
        isJump = false;
        isSlide = false;
    }
    public void PlayerMove()
    {
        if(!isSlide&&!isJump&&!isDie)
        {
            //if(Input.GetKeyDown(KeyCode.A))
            //{
            //    Debug.Log("왼쪽으로 움직일까요?");
            //    transform.position -= moveDir * rotationSpeed * Time.deltaTime;
            //    Debug.Log("왼쪽으로 움직였습니다.");
            //}
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //        Debug.Log("오른쪽으로 움직일까요?");
            //    transform.position += moveDir * rotationSpeed * Time.deltaTime;
            //        Debug.Log("오른쪽으로 움직였습니다.");
            //}

            //x = Input.GetAxisRaw("Horizontal");
            //Player_rig.AddForce(new Vector3(x, 0, 0));
            
            if(isStart)
            {
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    transform.position += transform.right * rotationSpeed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.position -= transform.right * rotationSpeed * Time.deltaTime;
                }
            }
           

        }
       
            
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
    public void PlayerDie()
    {
        isDie = true;
        


        if (result != null)
        {
            result.CallResult();
        }
        else
        {
            Debug.LogError("result 객체가 초기화되지 않았습니다.");
        }

        Time.timeScale = 0;
        bearAudio.Stop();

        player_Audio.Stop();
        //
    }
    public void RemoveFowardSpeed()
    {
        StartCoroutine("Remove_forward");
    }
    private IEnumerator Remove_forward()
    {
        isStart = false;
        forwardSpeed = 0;
        yield return new WaitForSeconds(4f);
        forwardSpeed = 1000;
        isStart = true;
    }
}
