using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class HitPointBar : MonoBehaviour
    {
        [SerializeField] private Image m_Image;
        private float m_LastHitPoints;
        void Update()
        {
            float hitpoints = (float)Player.Instance.GetComponent<Destructible>().HitPoints / (float)Player.Instance.GetComponent<Destructible>().MaxHitPoints;
            if (hitpoints != m_LastHitPoints)
            {
                m_Image.fillAmount = hitpoints;
                m_LastHitPoints = hitpoints;
            }
        }


    }


