using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_PointerEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnityEvent OnClickEvent;

    private void Start()
    {
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.1f;
        transform.localRotation = Quaternion.identity;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickEvent?.Invoke();
    }
}
