using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUpgradeShop : MonoBehaviour
{
  [SerializeField] private TMP_Text coinText;
  [SerializeField] private Button button;
  [SerializeField] private string key;
  [SerializeField] private int baseCost;

  [Header("Text")] [SerializeField] private TMP_Text currentLevelText;
  [SerializeField] private TMP_Text nextLevelText;
  [SerializeField] private TMP_Text costText;

  private int coinAmount;
  private int level;
  private int cost;
  
  private void Start()
  { 
    coinAmount = PlayerPrefs.GetInt("Coin");
    level = PlayerPrefs.GetInt(key);
    
    if (level == 0)
    {
      level = 1;
      cost = baseCost * baseCost;
    }
    else
      cost = baseCost * baseCost * level;
    
    TextChange();
  }

  public void DrawCoin()
  {
    if (coinAmount - cost < 0) return;

    coinAmount -= cost;
    coinText.text = coinAmount.ToString();
    level++;
    cost = baseCost * baseCost * level;

    PlayerPrefs.SetInt("Coin", coinAmount);
    PlayerPrefs.SetInt(key, level);
    PlayerPrefs.Save();

    TextChange();
  }

  private void Update()
  {
    SetButton();
    if(Input.GetKeyDown(KeyCode.R))
      DebugCoin();
  }

  private void SetButton()
  {
    if (level < 10)
    {
      button.interactable = (coinAmount >= cost);
    }
    else
    {
      button.interactable = false;
      nextLevelText.text = "Max";
      costText.text = "-";
    }
  }

  private void TextChange()
  {
    currentLevelText.text = level.ToString();
    nextLevelText.text = (level + 1).ToString();
    costText.text = cost.ToString();
  }

  private void DebugCoin()
  {
    PlayerPrefs.SetInt("Coin", 1000);
    PlayerPrefs.SetInt("Ability1",0 );
    PlayerPrefs.SetInt("Ability2",0 );
    PlayerPrefs.SetInt("Ability3",0 );
    PlayerPrefs.SetInt("Ability4",0 );
    PlayerPrefs.SetInt("Ability5",0 );
    PlayerPrefs.Save();
  }
}