using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private Bag bag;
    [SerializeField] private TMP_Text text;
    
    private void Update()
    {
        if(bag && text)
        text.text = bag.GetCoinAmount().ToString();
    }
}
