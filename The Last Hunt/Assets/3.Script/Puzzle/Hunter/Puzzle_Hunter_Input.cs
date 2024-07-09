using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Hunter_Input : MonoBehaviour
{
    /// <summary>
    /// x = ���� �� �Ÿ�
    /// y = ���� ����
    /// </summary>
    [SerializeField]
    Vector2 offset = new Vector2();

    private void OnDrawGizmos()
    {
        Vector3 drawPosition = transform.position + transform.forward * offset.x + Vector3.up * offset.y;

        Gizmos.DrawWireSphere(drawPosition, 1f);
    }

    /// <summary>
    /// Ÿ���� ��� �ִ°�?
    /// </summary>
    bool isCarrying = false;

    private void Update()
    {
        //�÷��̾� ���� Ư�� ��ġ���� Raycast�� ����
        Vector3 drawPosition = transform.position + transform.forward * offset.x + Vector3.up * offset.y;

        //�ƹ��͵� ��� ���� ���� �� �˻縦 ����
        if (isCarrying)
        {
            //�ڽ� ���� Ư�� �Ÿ��� �˻�
            if()
        }  
    }
}
