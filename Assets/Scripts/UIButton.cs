using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack).SetLink(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetLink(gameObject);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}