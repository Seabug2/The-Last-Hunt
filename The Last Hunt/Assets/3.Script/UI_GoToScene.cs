using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GoToScene : MonoBehaviour
{
    [SerializeField]
    private int sceneNumber; // 인스펙터에서 설정할 씬 번호 변수

    // UI 버튼에 연결할 메서드
    public void GoToScene()
    {
        DOTween.KillAll();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneNumber); // 설정된 씬 번호로 이동
    }
}
