using UnityEngine;

public class Puzzle_Obstacle : MonoBehaviour
{
    [Header("���ſ� �ʿ��� ������")]
    public string RequiredName;

    [SerializeField]
    GameObject prtc;

    private void OnDestroy()
    {
        if (prtc)
        {
            prtc.transform.parent = null;
            prtc.SetActive(true);
        }
    }
}
