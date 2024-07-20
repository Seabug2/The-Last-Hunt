using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_RockAndTree : Puzzle_Obstacle
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Puzzle_Horse horse))
        {
            //�� �� ���ӿ��� ������ ������ ������� ����
            if (Puzzle_GameManager.instance.IsGameOver) return;

            Puzzle_GameManager.instance.EndGame?.Invoke();
            Puzzle_GameManager.instance.VCamFollowHorse();
            Puzzle_GameManager.instance.GameOver_Horse(horse);
        }
    }
}
