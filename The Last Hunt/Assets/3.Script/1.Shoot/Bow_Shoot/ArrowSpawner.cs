using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private PlayerController_Shoot p_controller;
    [SerializeField] private PlayerInput_Shoot input;
    [SerializeField] private Transform spawn;
    [SerializeField] private GameObject arrowPrefab;
    private GameObject arrow;

    [SerializeField] private Archery_Data_Shoot archery_Data;
    private int ammoRemain = 0;
    private bool isKnock = false;
    private float arrowForce = 20f;
    private float drawTime = 0f;

    private void Start()
    {
        input = GetComponentInParent<PlayerInput_Shoot>();
        p_controller = GetComponentInParent<PlayerController_Shoot>();
        ammoRemain = p_controller.ammoRemain;
    }

    private void Update()
    {
        if (ammoRemain > 0)
        {
            if (input.isKnock)
            {
                isKnock = true;
            }
            if (input.isFire && isKnock)
            {
                arrow = Instantiate(arrowPrefab, spawn.position, spawn.rotation);
                drawTime = p_controller.releaseTime - p_controller.drawTime;
                if (drawTime > 5f)
                {
                    drawTime = 5f;
                }
                arrow.GetComponent<Rigidbody>().AddForce(transform.up * arrowForce * drawTime, ForceMode.Impulse);
                ammoRemain--;
                isKnock = false;
            }
        }
        else
        {
            return;
        }
    }
}
