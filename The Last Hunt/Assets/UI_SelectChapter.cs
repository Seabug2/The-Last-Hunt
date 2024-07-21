using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    string[] name;
    [SerializeField]
    Text ChapterName;

    private void Awake()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    public void GoToSelectChapter()
    {
        menuCamera.Priority = 0;
        selectedNum = 0;
        ChapterName.text = name[selectedNum];
        cameras[selectedNum].Priority = menuCamera.Priority + 1;
        popUpCanvas.SetActive(true);
    }

    public void ShiftVirtualCamera(int num)
    {
        brain.ActiveVirtualCamera.Priority = 0;
        selectedNum = selectedNum + num;

        if (selectedNum < 0 || selectedNum >= cameras.Length)
        {
            GoBackMenu();
        }

        ChapterName.text = name[selectedNum];
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
        SceneManager.LoadScene(SceneNumber[selectedNum]);
    }

}
