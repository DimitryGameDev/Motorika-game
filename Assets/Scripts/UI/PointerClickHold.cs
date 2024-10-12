using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerClickHold : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Button button;

    private bool m_Hold;
    public bool IsHold => m_Hold;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if(button != null && button.interactable)
        m_Hold = true;
        else
        m_Hold = false;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        m_Hold = false;
    }
}