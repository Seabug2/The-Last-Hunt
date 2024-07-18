using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController_Shoot : MonoBehaviour
{
    [SerializeField] private PlayerController_Shoot p_controller;
    [SerializeField] private PlayerInput_Shoot input;
    [SerializeField] private Animator bow_ani;

    public GameObject arrowPrefab;
    public int ammo;
    private bool isKnock = false;

    private void Start()
    {
        input = GetComponentInParent<PlayerInput_Shoot>();
        p_controller = GetComponentInParent<PlayerController_Shoot>();
        TryGetComponent(out bow_ani);
        arrowPrefab.SetActive(false);
        ammo = p_controller.ammoRemain;
    }
    
    private void Update()
    {
        if (ammo > 0)
        {
            if (input.isKnock)
            {
                arrowPrefab.SetActive(true);
                bow_ani.SetBool("isDraw", true);
                isKnock = true;
            }
            else if (input.isKnockCancel)
            {
                arrowPrefab.SetActive(false);
                bow_ani.SetBool("isDraw", false);
                isKnock = false;
            }
            if (input.isFire && isKnock)
            {
                arrowPrefab.SetActive(false);
                bow_ani.SetBool("isDraw", false);
                ammo--;
                isKnock = false;
                Debug.Log($"Remaining : {ammo}");
            }
        }
        else
        {
            return;
        }
    }
    
}
