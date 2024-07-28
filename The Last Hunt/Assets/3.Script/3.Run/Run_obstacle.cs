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
        player = player_o.GetComponent<Run_PlayerMove>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Equals("Player"))
        {
            player.PlayerDie();
        }
    }
}
