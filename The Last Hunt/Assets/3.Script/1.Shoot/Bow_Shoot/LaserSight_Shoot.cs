using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight_Shoot : MonoBehaviour
{
    [SerializeField] private LineRenderer laser;

    private void Awake()
    {
        TryGetComponent(out laser);

        laser.positionCount = 2;
        laser.enabled = false;
    }

    private void FixedUpdate()
    {
        laser.enabled = true;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, transform.position + transform.forward * 10);
    }
}
