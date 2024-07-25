using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_GoToScene : MonoBehaviour
{
    [SerializeField]
    Image fadeBoard;

    /// <summary>
    /// 다음 씬으로 넘어가기 전 실행해야 하는 것들이 있으면 이 이벤트에 추가해주세요.
    /// 추가 방법은 FadeOutEvent.AddListener() 라는 것을 쓰면 됩니다.
    /// </summary>
    public UnityEvent FadeOutEvent;

    // UI 버튼에 연결할 메서드
    public void GoToScene(int sceneNumber)
    {
        FadeOutEvent?.Invoke();
        DOTween.KillAll();
        Time.timeScale = 1.0f;
        if (fadeBoard)
        {
            fadeBoard.color = new Color(0, 0, 0, 0);
            fadeBoard.gameObject.SetActive(true);
            fadeBoard.DOFade(1, 3.5f).OnComplete(() => SceneManager.LoadScene(sceneNumber));
        }
        else
        {
            SceneManager.LoadScene(sceneNumber); // 설정된 씬 번호로 이동
        }
    }
}
