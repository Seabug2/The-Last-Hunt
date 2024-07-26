using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Shoot : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private PlayerInput_Shoot input;
    private Rigidbody player_r;
    [SerializeField] private Animator player_ani;
    [SerializeField] private AudioSource audio_s;

    [SerializeField] private MapData_Shoot Map_Data;
    [SerializeField] private Archery_Data_Shoot Archery_Data;
    public int ammoRemain = 0;

    public float drawTime = 0.0f;
    public float releaseTime = 0.0f;
    private bool isKnock = false;
    private bool isMoving;
    public bool isDraw;
    public bool isAlive = true;


    private void Awake()
    {
        TryGetComponent(out input);
        TryGetComponent(out player_r);
        TryGetComponent(out audio_s);
        player_ani = GetComponentInChildren<Animator>();
        ammoRemain = Archery_Data.QuiverCapacity;
        isDraw = false;
        isMoving = false;
        moveSpeed = 8f;
    }

    private void Update()
    {
        MoveFB();
        MoveLR();
        Rotate();

        if (input.MoveFBValue > 0.1 || input.MoveFBValue < -0.1)
        {
            player_ani.SetFloat("MoveFB", input.MoveFBValue);
            isMoving = true;
        }
        else if (input.MoveLRValue > 0.1 || input.MoveLRValue < -0.1)
        {
            player_ani.SetFloat("MoveLR", input.MoveLRValue);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        if (isMoving)
        {
            if (!audio_s.isPlaying)
            {
                audio_s.Play();
            }
        }
        else
        {
            audio_s.Stop();
        }

        if (ammoRemain <= 0)
        {
            player_ani.SetBool("isKnock", false);
            player_ani.SetBool("isDraw", false);
        }
        else
        {
            if (input.isKnock)
            {
                moveSpeed = moveSpeed * 0.5f;
                player_ani.SetBool("isKnock", true);
                isKnock = true;
            }
            if (input.isKnockCancel)
            {
                moveSpeed = 8f;
                player_ani.SetBool("isKnock", false);
                isKnock = false;
                isDraw = false;
                drawTime = 0;
            }
            if (input.isDraw && !isDraw && isKnock)
            {
                moveSpeed = moveSpeed * 0.5f;
                player_ani.SetBool("isDraw", true);
                drawTime = Time.time;
                isDraw = true;
            }
            if (input.isFire && isKnock)
            {
                moveSpeed = 8f;
                ammoRemain--;
                player_ani.SetTrigger("Fire");
                player_ani.SetBool("isDraw", false);
                releaseTime = Time.time;
                isDraw = false;
                isKnock = false;
                Debug.Log($"Draw time : {releaseTime - drawTime}");
                Debug.Log($"Remaining (Player) : {ammoRemain}");
            }
        }
    }
    
    private void MoveFB()
    {
        Vector3 MoveDirection = input.MoveFBValue * transform.forward * moveSpeed * Time.deltaTime;
        player_r.MovePosition(player_r.position + MoveDirection);
        player_r.position = new Vector3(Mathf.Clamp(player_r.position.x, Map_Data.LimitMin.x, Map_Data.LimitMax.x), Mathf.Clamp(player_r.position.y, Map_Data.LimitMin.y, Map_Data.LimitMax.y), Mathf.Clamp(player_r.position.z, Map_Data.LimitMin.z, Map_Data.LimitMax.z));
    }

    private void MoveLR()
    {
        Vector3 MoveDirection = input.MoveLRValue * transform.right * moveSpeed * Time.deltaTime;
        player_r.MovePosition(player_r.position + MoveDirection);
        player_r.position = new Vector3(Mathf.Clamp(player_r.position.x, Map_Data.LimitMin.x, Map_Data.LimitMax.x), Mathf.Clamp(player_r.position.y, Map_Data.LimitMin.y, Map_Data.LimitMax.y), Mathf.Clamp(player_r.position.z, Map_Data.LimitMin.z, Map_Data.LimitMax.z));
    }

    private void Rotate()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}
