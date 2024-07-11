using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Hunter_Movement : Puzzle_Movement
{
    Animator anim;

    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    [SerializeField, Header("�̵� �ӵ��� ȸ�� �ӵ�"), Space(10)]
    protected float moveSpeed = 1;
    /// <summary>
    /// ȸ���ӵ�
    /// </summary>
    [SerializeField]
    protected float rotSpeed = 1;
    /// <summary>
    /// �̵��� ����
    /// </summary>
    [HideInInspector]
    public Vector3 dir = Vector3.zero;

    protected new void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
        {
            anim.SetBool("isMoving", true);
        }
        else if (!Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void FixedUpdate()
    {
        if (dir != Vector3.zero)
        {
            transform.position += Time.fixedDeltaTime * moveSpeed * dir;
            Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotSpeed);
        }

        //�� �Ʒ��� sphere ������ŭ�� �����Ͽ� Ÿ���� ���ٸ� �������ϴ�.
        if (Physics.OverlapBox(transform.position, floorCheckerSize, Quaternion.identity, tileLayer).Length == 0)
        {
            Falling();
        }
    }

    public override void Falling()
    {
        base.Falling();
        //�� �̻� ���� �Ұ�
        anim.SetTrigger("Falling");
        GetComponent<Puzzle_Hunter_Input>().enabled = false;
        GetComponent<Puzzle_Hunter_Carrying>().enabled = false;
        rb.AddForce(dir.normalized, ForceMode.Impulse);
    }

    [SerializeField, Header("�ٴ� ���� ���� ũ��"), Space(10)]
    Vector3 floorCheckerSize;

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        //���� Ÿ�� �˻�
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, floorCheckerSize);
    }
#endif
}
