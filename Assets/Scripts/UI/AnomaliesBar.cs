using UnityEngine;
using UnityEngine.UI;

public class AnomaliesBar : MonoBehaviour
{
    [SerializeField] private Bag bag;
    [SerializeField] private AbilitiesChanger abilitiesChanger;
    [SerializeField] private Image image;
    
    private void Update()
    {
        if (bag && abilitiesChanger)
        {
            image.fillAmount = (float)bag.GetAnomaliesAmount() / (float)abilitiesChanger.CountForChange;
        }
    }
}