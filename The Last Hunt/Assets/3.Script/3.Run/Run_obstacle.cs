using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_obstacle : MonoBehaviour
{
    Run_PlayerMove player;
    [SerializeField] GameObject player_o;

    private void Start()
    {
        player_o = GameObject.Find("Player");
        if (player_o == null)
        {
            Debug.LogError("Player GameObject�� ã�� �� �����ϴ�.");
            return;
        }

        player = player_o.GetComponent<Run_PlayerMove>();
        if (player == null)
        {
            Debug.LogError("Player GameObject�� Run_PlayerMove ������Ʈ�� �����ϴ�.");
        }
    }

    
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Equals("Player"))
        {
            if (player != null)
            {
                player.PlayerDie();
            }
            else
            {
                Debug.LogError("player ������ null�Դϴ�.");
            }
        }
    }
}
