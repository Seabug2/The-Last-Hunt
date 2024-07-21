using DG.Tweening;
using UnityEngine;

public class UI_Quit : MonoBehaviour
{
    // UI 버튼에 연결할 메서드
    public void QuitGame()
    {
        DOTween.KillAll();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 상태에서 플레이 중단
#else
        Application.Quit(); // 빌드된 게임에서는 게임 종료
#endif
    }
}
