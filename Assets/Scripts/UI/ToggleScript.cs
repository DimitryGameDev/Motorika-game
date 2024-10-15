using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private Sprite[] buttonSprites;
    [SerializeField] private Image targetButton;

    private int ID;

    private void Start()
    {
        ID = PlayerPrefs.GetInt(key);

        if (ID == 0)
            targetButton.sprite = buttonSprites[0];
        else
            targetButton.sprite = buttonSprites[1];
    }

    public void ChangeSprite()
    {
        if (targetButton.sprite == buttonSprites[0])
        {
            targetButton.sprite = buttonSprites[1];
            PlayerPrefs.SetInt(key, 1);
            return;
        }
        targetButton.sprite = buttonSprites[0];

        PlayerPrefs.SetInt(key, 0);
        Debug.Log(PlayerPrefs.GetInt(key));
        PlayerPrefs.Save();
    }
}