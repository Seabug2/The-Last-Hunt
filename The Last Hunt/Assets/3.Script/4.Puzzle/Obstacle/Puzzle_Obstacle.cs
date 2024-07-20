using UnityEngine;

public class Puzzle_Obstacle : MonoBehaviour
{
    [Header("���ſ� �ʿ��� ������")]
    public string RequiredName;

    [SerializeField]
    GameObject prtc;

    public void Dead()
    {
        if (prtc)
        {
            prtc.transform.parent = null;
            prtc.SetActive(true);
        }
        Destroy(gameObject);
    }
}
