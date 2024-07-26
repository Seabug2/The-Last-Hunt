using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Rhythm_ButtonController : MonoBehaviour
{
    public void DebugButton()
    {
        Rhythm_ChapterManager.instance.DebugButton();
    }
}
