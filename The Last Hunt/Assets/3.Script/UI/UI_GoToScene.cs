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
    /// ���� ������ �Ѿ�� �� �����ؾ� �ϴ� �͵��� ������ �� �̺�Ʈ�� �߰����ּ���.
    /// �߰� ����� FadeOutEvent.AddListener() ��� ���� ���� �˴ϴ�.
    /// </summary>
    public UnityEvent FadeOutEvent;

    // UI ��ư�� ������ �޼���
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
            SceneManager.LoadScene(sceneNumber); // ������ �� ��ȣ�� �̵�
        }
    }
}
