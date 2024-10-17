using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerClickHold : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    [SerializeField] private Button button;
    public event UnityAction Click;

    private bool m_Hold;
    public bool IsHold => m_Hold;

    private int levelID;
    private Vector2 startPosition;
    private void Start()
    {
        startPosition = transform.position;
        SetButtonPosition();
    }

    public void SetButtonPosition()
    {
        levelID = PlayerPrefs.GetInt("Control");

        if (levelID == 0)
        {
            transform.position = startPosition;
        }
        else
        {
            transform.position = new Vector2(Screen.width - transform.position.x, transform.position.y);
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (button != null && button.interactable)
            m_Hold = true;
        else
            m_Hold = false;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        m_Hold = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        Click?.Invoke();
    }
}