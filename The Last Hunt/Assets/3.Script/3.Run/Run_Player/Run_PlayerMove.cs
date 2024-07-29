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


        //���߿� �ڳ� ������ transform.Forward�� ���ǿ� �°� �ٲ����.
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
        Debug.Log("PlayerJump ����");
        if (!isJump && !isSlide)
        {
            isJump = true;
            Debug.Log("isJump�� True�Դϴ�.");
            player_ani.SetBool("isJump", true);
            player_ani.SetBool("isRun", false);
            Debug.Log("isJump_animation�� True�Դϴ�.");
            player_rig.AddForce(new Vector3(0, JumpForce, 0), ForceMode.VelocityChange);

        }

    }
    public void EndJump()//���� animation KeyFrame�� �߰��� �޼���
    {
        if (isDie) return;
        isJump = false;
        player_ani.SetBool("isJump", false);
        player_ani.SetBool("isRun", true);
        Debug.Log("isJump�� false�Դϴ�.");
    }

    public void PlayerSlide()
    {
        if (isDie) return;
        isSlide = true;
        player_ani.SetBool("isSlide", true);
        player_ani.SetBool("isRun", false);
        player_c.height = 1;
        Debug.Log("isSlide�� True�Դϴ�.");
    }
    public void EndSlide()//�����̵� animation Keyframe�� �߰��� �޼���
    {
        if (isDie) return;
        isSlide = false;
        player_ani.SetBool("isSlide", false);
        player_ani.SetBool("isRun", true);
        player_c.height = 2;
        Debug.Log("isSlide�� false�Դϴ�.");

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
            //    Debug.Log("�������� �����ϱ��?");
            //    transform.position -= moveDir * rotationSpeed * Time.deltaTime;
            //    Debug.Log("�������� ���������ϴ�.");
            //}
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //        Debug.Log("���������� �����ϱ��?");
            //    transform.position += moveDir * rotationSpeed * Time.deltaTime;
            //        Debug.Log("���������� ���������ϴ�.");
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
    //     Debug.Log("�� ���� ������ �ߴµ���");
    //     Player_rig.AddForce(new Vector3(0, JumpForce, 0));
    //     player_ani.SetBool("isJump", true);
    //     player_ani.SetBool("isRun", false);
    //
    //     isJump = true;
    //     jumpCount = 0;
    //     yield return new WaitForSeconds(0.5f);
    //     Debug.Log("�� ���� �Ѿ���� �ߴµ���");
    //     player_ani.SetBool("isJump", false);
    //     player_ani.SetBool("isRun", true);
    //     jumpCount = 1;
    //     isJump = false;
    //     Debug.Log("�� ���� ������ �Դµ���");
    //     
    // 
    // }
    // private IEnumerator Slide_co()
    // {
    //     
    //     Debug.Log("�� �����̵� ������ �ߴµ���");
    //    
    //         player_ani.SetBool("isSlide", true);
    //         player_ani.SetBool("isRun", false);
    //         isSlide = true;
    //         player_c.height = 1;
    //         slideCount=0;
    //         yield return new WaitForSeconds(1f);
    //         Debug.Log("�� �����̵� �Ѿ���� �ߴµ���");
    //         player_ani.SetBool("isSlide", false);
    //         player_ani.SetBool("isRun", true);
    //         player_c.height = 2;
    //         slideCount = 1;
    //         isSlide = false;
    //         Debug.Log("�� �����̵� ������ �Դµ���");
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
            Debug.LogError("result ��ü�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
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
