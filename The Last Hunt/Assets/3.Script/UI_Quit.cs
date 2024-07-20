using DG.Tweening;
using UnityEngine;

public class UI_Quit : MonoBehaviour
{
    // UI ��ư�� ������ �޼���
    public void QuitGame()
    {
        DOTween.KillAll();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ������ ���¿��� �÷��� �ߴ�
#else
        Application.Quit(); // ����� ���ӿ����� ���� ����
#endif
    }
}
