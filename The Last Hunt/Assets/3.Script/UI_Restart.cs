using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UI_Restart : MonoBehaviour
{
    // UI ��ư�� ������ �޼���
    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        DOTween.KillAll();
        Scene currentScene = SceneManager.GetActiveScene(); // ���� �� ��������
        SceneManager.LoadScene(currentScene.buildIndex); // ���� �� �ٽ� �ε�
    }
}