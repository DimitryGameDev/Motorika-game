using UnityEngine;
using UnityEngine.UI;

public class HitPointBar : MonoBehaviour
{
    [SerializeField] private Destructible destructible;
    [SerializeField] private Image image;
    
    private void Update()
    {
        if (destructible)
        {
            image.fillAmount = (float)destructible.HitPoints / (float)destructible.MaxHitPoints;
        }
    }
}