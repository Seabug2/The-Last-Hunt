using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Rhythm_ButtonController : MonoBehaviour
{
    public void DebugButton()
    {
        Rhythm_ChapterManager.instance.DebugButton();
    }

    public void SceneLoader(int num)
    {
        if (num > 4 && Rhythm_ChapterManager.instance.percent < 60)
        {
            return;
        }
        Time.timeScale = 1.0f;
        DOTween.KillAll();

        if (num.Equals(0))
        {
            SceneManager.LoadScene(0);
        }
        else if (num.Equals(4))
        {
            Scene currentScene = SceneManager.GetActiveScene(); // 현재 씬 가져오기
            SceneManager.LoadScene(currentScene.buildIndex); // 현재 씬 다시 로드
        }
        else
        {
            print("다음 씬으로");
        }
    }
}
