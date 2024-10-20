using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerClickHold : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button button;

    private bool isHold;
    public bool IsHold => isHold;
    public event UnityAction Click;

    public void SetButtonPosition(Transform transform)
    {
        button.transform.position = transform.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button != null && button.interactable)
            Click?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHold = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHold = false;
    }
}