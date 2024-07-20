using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UI_Restart : MonoBehaviour
{
    // UI 버튼에 연결할 메서드
    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        DOTween.KillAll();
        Scene currentScene = SceneManager.GetActiveScene(); // 현재 씬 가져오기
        SceneManager.LoadScene(currentScene.buildIndex); // 현재 씬 다시 로드
    }
}