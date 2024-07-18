using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput_Shoot : MonoBehaviour
{
    [SerializeField] private string MoveFB_name = "Vertical";
    [SerializeField] private string MoveLR_name = "Horizontal";
    [SerializeField] private string Draw_name = "Fire1";
    [SerializeField] private string Knock_name = "Fire3";


    public float MoveFBValue { get; private set; }
    public float MoveLRValue { get; private set; }

    public bool isFire { get; private set; }
    public bool isDraw { get; private set; }
    public bool isKnock { get; private set; }
    public bool isKnockCancel { get; private set; }

    private void Update()
    {
        // Movement
        MoveFBValue = Input.GetAxis(MoveFB_name);
        MoveLRValue = Input.GetAxis(MoveLR_name);
        // Load arrow
        isKnock = Input.GetButtonDown(Knock_name);
        isKnockCancel = Input.GetButtonUp(Knock_name);
        // Pull & aim
        isDraw = Input.GetButtonDown(Draw_name);
        // Loose arrow
        isFire = Input.GetButtonUp(Draw_name);
    }
}
