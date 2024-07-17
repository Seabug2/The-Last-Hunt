using UnityEngine;

public class Puzzle_Obstacle : MonoBehaviour
{
    [Header("제거에 필요한 아이템")]
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
