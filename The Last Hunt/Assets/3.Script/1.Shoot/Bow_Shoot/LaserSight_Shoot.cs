using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight_Shoot : MonoBehaviour
{
    [SerializeField] private LineRenderer laser;
    [SerializeField] private PlayerInput_Shoot input;

    private void Awake()
    {
        TryGetComponent(out laser);
        input = GetComponentInParent<PlayerInput_Shoot>();

        laser.positionCount = 2;
        laser.enabled = false;
    }

    private void Update()
    {
        laser.enabled = true;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, transform.position + transform.forward * 20);
    }
}
