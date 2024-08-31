using UnityEngine;
using UnityEngine.Events;

public class Bag : MonoBehaviour
{
    private int coinAmount;

    public UnityEvent ChangeCoinAmount;
    public void AddCoin(int amount)
    {
        coinAmount += amount;
        ChangeCoinAmount?.Invoke();
    }

    public bool DrawCoin(int amount)
    {
        if (coinAmount - amount < 0) return false;

        coinAmount -= amount;
        ChangeCoinAmount?.Invoke();

        return true;
    }

    public int GetCoinAmount()
    {
        return coinAmount;
    }
}