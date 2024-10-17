using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUpgradeShop : MonoBehaviour
{
  [SerializeField] private TMP_Text coinText;
  [SerializeField] private Button button;
  [SerializeField] private string key;
  [SerializeField] private int cost;
  
  private int coinAmount;
  private int level;
  
  private void Start()
  {
    coinAmount = PlayerPrefs.GetInt("Coin");
    
    level = PlayerPrefs.GetInt(key);
    if (level == 0)
      level = 1;
  }

  public void DrawCoin()
  {
    if (coinAmount - cost < 0) return;

    coinAmount -= cost;
    coinText.text = coinAmount.ToString();
    level++;
    cost *= level;
    
    PlayerPrefs.SetInt("Coin", coinAmount);
    PlayerPrefs.SetInt(key, level);
    PlayerPrefs.Save();
  }

  private void Update()
  {
    button.interactable = (coinAmount >= cost);
    
    Debug.Log(key + level + " cost " + cost + " Coinamount "+ coinAmount);
  }
}
