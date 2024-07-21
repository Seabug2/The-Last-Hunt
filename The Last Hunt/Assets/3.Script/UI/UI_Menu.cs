using UnityEngine;

public class UI_Menu : MonoBehaviour
{
    [SerializeField]
    GameObject[] Page;
    int currentPage;

    private void Awake()
    {
        foreach (GameObject g in Page) 
        {
            g.SetActive(false);
        }

        currentPage = 1;
        Page[currentPage].SetActive(true);
    }

    public void ShiftPage(int dir)
    {
        Page[currentPage].SetActive(false);
        currentPage = Mathf.Clamp(currentPage + dir, 0, Page.Length - 1);
        Page[currentPage].SetActive(true);
    }
}
