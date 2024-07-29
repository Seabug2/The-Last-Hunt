using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_obstacle : MonoBehaviour
{
    Run_PlayerMove player_r;
    [SerializeField] GameObject player_o;

    private void Start()
    {
        player_o = GameObject.Find("Player");
        if (player_o == null)
        {
            Debug.LogError("Player GameObject�� ã�� �� �����ϴ�.");
            return;
        }

        player_r = player_o.GetComponent<Run_PlayerMove>();
        if (player_o == null)
        {
            Debug.LogError("Player GameObject�� Run_PlayerMove ������Ʈ�� �����ϴ�.");
        }
    }

    
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            if (player_r != null)
            {
                player_r.PlayerDie();
            }
            else
            {
                Debug.LogError("player ������ null�Դϴ�.");
            }
        }
    }
}
