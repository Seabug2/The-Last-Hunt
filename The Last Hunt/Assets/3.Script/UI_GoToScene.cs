using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GoToScene : MonoBehaviour
{
    [SerializeField]
    private int sceneNumber; // �ν����Ϳ��� ������ �� ��ȣ ����

    // UI ��ư�� ������ �޼���
    public void GoToScene()
    {
        DOTween.KillAll();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneNumber); // ������ �� ��ȣ�� �̵�
    }
}
