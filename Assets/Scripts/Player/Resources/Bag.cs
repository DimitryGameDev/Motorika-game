using UnityEngine;
using UnityEngine.Events;

public class Bag : MonoBehaviour
{
    private int coinAmount;
    private int anomaliesAmount;

    public UnityEvent ChangeCoinAmount;
    public UnityEvent ChangeAnomaliesAmount;

    private void Update()
    {
      //  Debug.Log("Coins: " + coinAmount + "Anomalies: " + anomaliesAmount);
    }

    public void AddCoin(int amount)
    {
        coinAmount += amount;
        ChangeCoinAmount?.Invoke();
    }

    public void AddAnomalies(int amount)
    {
        anomaliesAmount += amount;
        ChangeAnomaliesAmount?.Invoke();
    }

    public bool DrawCoin(int amount)
    {
        if (coinAmount - amount < 0) return false;

        coinAmount -= amount;
        ChangeCoinAmount?.Invoke();

        return true;
    }

    public bool DrawAnomalies(int amount)
    {
        if (anomaliesAmount - amount < 0) return false;

        anomaliesAmount -= amount;
        ChangeAnomaliesAmount?.Invoke();

        return true;
    }

    public int GetCoinAmount()
    {
        return coinAmount;
    }

    public int GetAnomaliesAmount()
    {
        return anomaliesAmount;
    }
}