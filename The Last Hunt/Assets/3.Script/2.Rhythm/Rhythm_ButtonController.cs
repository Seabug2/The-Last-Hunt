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
                print("��������");
                break;
            case 4:
                print("�ٽ��ϱ�");
                break;
            case 5:
                if(Rhythm_ChapterManager.instance.percent > 59)
                {
                    print("��������");
                }
                break;
        }
    }
}
