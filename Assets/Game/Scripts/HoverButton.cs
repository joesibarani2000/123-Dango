using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Range(0f, 2f)] private float scaleValue = 1.1f;
    [SerializeField] UnityEvent onEnterEvent, onExitEvent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one * scaleValue, 0.1f).SetEase(Ease.InBounce);
        AudioController.PlaySFX("Button_Select");

        onEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InBounce);

        onExitEvent?.Invoke();
    }
}
