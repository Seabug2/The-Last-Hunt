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
            Debug.LogError("Player GameObject를 찾을 수 없습니다.");
            return;
        }

        player = player_o.GetComponent<Run_PlayerMove>();
        if (player == null)
        {
            Debug.LogError("Player GameObject에 Run_PlayerMove 컴포넌트가 없습니다.");
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
                Debug.LogError("player 변수가 null입니다.");
            }
        }
    }
}
