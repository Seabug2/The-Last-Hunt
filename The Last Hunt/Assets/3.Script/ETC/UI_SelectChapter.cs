using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UI_SelectChapter : MonoBehaviour
{
    int selectedNum = 0;
    [SerializeField]
    GameObject popUpCanvas;
    [SerializeField]
    CinemachineVirtualCamera[] cameras;
    [SerializeField]
    CinemachineVirtualCamera menuCamera;
    CinemachineBrain brain;
    [SerializeField]
    int[] SceneNumber;
    [SerializeField]
    Sprite[] chapterSprtie;
    [SerializeField]
    Image chapterName;
    [SerializeField]
    GameObject isCleared;
    private void Awake()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    public void GoToSelectChapter()
    {
        menuCamera.Priority = 0;
        selectedNum = 0;
        chapterName.sprite = chapterSprtie[selectedNum];
        chapterName.SetNativeSize();
        isCleared.SetActive(GameManager.instance.userData.IsCleared[selectedNum]);
        cameras[selectedNum].Priority = menuCamera.Priority + 1;
        popUpCanvas.SetActive(true);
    }

    public void ShiftVirtualCamera(int num)
    {
        if (brain.IsBlending) return;

        brain.ActiveVirtualCamera.Priority = 0;
        selectedNum += num;

        if (selectedNum < 0 || selectedNum >= cameras.Length)
        {
            GoBackMenu();
            return;
        }

        chapterName.sprite = chapterSprtie[selectedNum];
        chapterName.SetNativeSize();
        isCleared.SetActive(GameManager.instance.userData.IsCleared[selectedNum]);
        cameras[selectedNum].Priority = brain.ActiveVirtualCamera.Priority + 1;
    }
    public void GoBackMenu()
    {
        brain.ActiveVirtualCamera.Priority = 0;
        menuCamera.Priority = brain.ActiveVirtualCamera.Priority + 1;
        popUpCanvas.SetActive(false);
    }

    public void GotoChapter()
    {
        if (selectedNum < 0 || selectedNum >= cameras.Length) return;
        if (fadeBoard)
        {
            fadeBoard.color = new Color(0, 0, 0, 0);
            fadeBoard.gameObject.SetActive(true);
            fadeBoard.DOFade(1, 4f).OnComplete(() => SceneManager.LoadScene(SceneNumber[selectedNum]));
        }
        else SceneManager.LoadScene(SceneNumber[selectedNum]);
    }

    public Image fadeBoard;
}
