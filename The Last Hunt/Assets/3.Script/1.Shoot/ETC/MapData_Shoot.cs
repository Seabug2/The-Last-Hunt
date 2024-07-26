using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapData_Shoot : ScriptableObject
{
    [SerializeField] private Vector3 _LimitMin;
    [SerializeField] private Vector3 _LimitMax;

    public Vector3 LimitMax
    {
        get
        {
            return _LimitMax;
        }
    }
    public Vector3 LimitMin
    {
        get
        {
            return _LimitMin;
        }
    }
}
