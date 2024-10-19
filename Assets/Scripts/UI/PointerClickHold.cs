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
    
    private Vector2 leftPosition;
    private Vector2 rightPosition;

    private void Start()
    {
        leftPosition = transform.position;
        rightPosition= new Vector2(Screen.width - transform.position.x, transform.position.y);
    }

    public void SetButtonPositionLeft()
    {
        transform.position = leftPosition;
    }

    public void SetButtonPositionRight()
    {
        transform.position = rightPosition;
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