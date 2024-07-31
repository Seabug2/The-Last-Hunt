using UnityEngine;

public class Puzzle_Skip : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Puzzle_GameManager.instance.IsGameOver) return;
            GameManager.instance.isStoryMode = true;
            StartCoroutine(Puzzle_GameManager.instance.GameClear_co());
        }
    }
}
