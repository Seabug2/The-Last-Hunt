using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rhythm_ButtonController : MonoBehaviour
{
    // 임시용 버튼 매니저
    public static void RestartButton()
    {
        Destroy(Rhythm_ChapterManager.instance);
        SceneManager.LoadScene("SampleScene");
    }

    public static void NextButton()
    {
        print("다음 씬으로");
    }

}
