using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rhythm_ButtonController : MonoBehaviour
{
    // �ӽÿ� ��ư �Ŵ���
    public static void RestartButton()
    {
        Destroy(Rhythm_ChapterManager.instance);
        SceneManager.LoadScene("SampleScene");
    }

    public static void NextButton()
    {
        print("���� ������");
    }

}
