using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Run_UIPopUp : MonoBehaviour
{
    RectTransform[] child;
    private void Start()
    {
        child = transform.GetComponentsInChildren<RectTransform>();
        for (int i = 0; i < child.Length; i++)
        {
            child[i].localScale = Vector3.zero;
            child[i].DOScale(Vector3.one, .5f).SetDelay(.25f + .125f * i).SetEase(Ease.OutBack);
        }
    }
}
