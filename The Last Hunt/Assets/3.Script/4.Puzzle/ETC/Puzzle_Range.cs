using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Puzzle_Range : MonoBehaviour
{
    [SerializeField]
    Transform leftDown;
    [SerializeField]
    Transform RightUp;

    Vector3 _leftDown;
    Vector3 _RightUp;

    Vector3 point1;
    Vector3 point2;
    Vector3 point3;
    Vector3 point4;

    [SerializeField]
    bool reset = false;
#if UNITY_EDITOR
    private void OnValidate()
    {
        reset = false;
        if (leftDown == null)
        {
            _leftDown = Vector3.zero;
        }
        else
        {
            _leftDown = leftDown.position - new Vector3(1.5f, 0, 1.5f);
        }
        if (RightUp == null)
        {
            _RightUp = Vector3.zero;
        }
        else
        {
            _RightUp = RightUp.position + new Vector3(1.5f, 0, 1.5f);
        }
        point1 = new Vector3(_leftDown.x, 0, _leftDown.z);
        point2 = new Vector3(_leftDown.x, 0, _RightUp.z);
        point3 = new Vector3(_RightUp.x, 0, _RightUp.z);
        point4 = new Vector3(_RightUp.x, 0, _leftDown.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(point1, point2);
        Gizmos.DrawLine(point2, point3);
        Gizmos.DrawLine(point3, point4);
        Gizmos.DrawLine(point4, point1);
    }
#endif
}