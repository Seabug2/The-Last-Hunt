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
        //만약 캐릭터가 점프 중이거나 슬라이딩 중이면 조작을 할 수 없다.
        if (move.isJump || move.isSlide) return;

        //만약 스페이스바를 눌렀으면
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //캐릭터가 점프를 한다.
            move.PlayerJump();
            Debug.Log("점프");
            return;
        }

        //만약 s 키를 눌렀으면
        if (Input.GetKeyDown(KeyCode.S))
        {
            //슬라이딩을 실행
            move.PlayerSlide();
            Debug.Log("슬라이드");
            return;
        }

        //if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        //{

        //좌우 입력을 했을 때
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 바닥 검사를 실시
            if (Physics.Raycast(transform.position, Vector3.down, Mathf.Infinity, targetLayer))
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    //우회전
                    transform.Rotate(new Vector3(0f, 90f, 0f));
                    return;
                }
                
                else if(Input.GetKeyDown(KeyCode.RightArrow))
                {
                    //좌회전
                    transform.Rotate(new Vector3(0f, -90f, 0f));
                    return;
                }
                
            }
        }


        //}
    }
}
