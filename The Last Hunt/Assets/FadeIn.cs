using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class FadeIn : MonoBehaviour
{
    Image fadeBoard;

    private void Awake()
    {
        fadeBoard = GetComponent<Image>();
    }
    private void Start()
    {
        fadeBoard.color = Color.black;
        fadeBoard.DOFade(0, 5).OnComplete(() => gameObject.SetActive(false));
    }
}
