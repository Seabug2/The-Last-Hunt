using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rhythm_ButtonController : MonoBehaviour
{
    public void SceneLoader(int num)
    {
        switch(num)
        {
            case 0:
                print("메인으로");
                break;
            case 4:
                print("다시하기");
                break;
            case 5:
                if(Rhythm_ChapterManager.instance.percent > 59)
                {
                    print("다음으로");
                }
                break;
        }
    }
}
