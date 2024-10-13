using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerClickHold : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Button button;

    private bool m_Hold;
    public bool IsHold => m_Hold;

    private int levelID;

    private void Start()
    {
        levelID = PlayerPrefs.GetInt("Control");

        if (levelID == 0)
        {
            transform.position = new Vector2(0 + transform.position.x, transform.position.y);
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
}