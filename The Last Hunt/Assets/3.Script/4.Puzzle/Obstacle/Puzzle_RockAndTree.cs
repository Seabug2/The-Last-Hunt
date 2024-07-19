using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_RockAndTree : Puzzle_Obstacle
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Puzzle_Horse _))
        {
            //한 번 게임오버 판정을 받으면 실행되지 않음
            if (Puzzle_GameManager.instance.IsGameOver) return;

            Puzzle_GameManager.instance.EndGame?.Invoke();
            Puzzle_GameManager.instance.VCamFollowHorse();;
        }
    }
}
