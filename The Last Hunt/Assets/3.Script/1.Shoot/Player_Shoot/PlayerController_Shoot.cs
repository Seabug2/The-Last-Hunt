using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Shoot : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private PlayerInput_Shoot input;
    private Rigidbody player_r;
    [SerializeField] private Animator player_ani;
    [SerializeField] private Archery_Data_Shoot Archery_Data;
    public int ammoRemain = 0;

    public float drawTime = 0.0f;
    public float releaseTime = 0.0f;
    private bool isKnock = false;
    public bool isDraw;
    public bool isAlive = true;
    public int killValue = 0;


    private void Awake()
    {
        TryGetComponent(out input);
        TryGetComponent(out player_r);
        player_ani = GetComponentInChildren<Animator>();
        ammoRemain = Archery_Data.QuiverCapacity;
        isDraw = false;
    }

    private void Update()
    {
        MoveFB();
        MoveLR();
        Rotate();

        if (input.MoveFBValue > 0.1 || input.MoveFBValue < -0.1)
        {
            player_ani.SetFloat("MoveFB", input.MoveFBValue);
        }
        if (input.MoveLRValue > 0.1 || input.MoveLRValue < -0.1)
        {
            player_ani.SetFloat("MoveLR", input.MoveLRValue);
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
                player_ani.SetBool("isKnock", true);
                isKnock = true;
            }
            else if (input.isKnockCancel)
            {
                player_ani.SetBool("isKnock", false);
                isKnock = false;
            }
            if (input.isDraw && !isDraw)
            {
                player_ani.SetBool("isDraw", true);
                drawTime = Time.time;
                isDraw = true;
            }
            if (input.isFire && isKnock)
            {
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
    }

    private void MoveLR()
    {
        Vector3 MoveDirection = input.MoveLRValue * transform.right * moveSpeed * Time.deltaTime;
        player_r.MovePosition(player_r.position + MoveDirection);
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
