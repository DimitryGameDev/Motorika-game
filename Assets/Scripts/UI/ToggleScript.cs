using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private Sprite[] buttonSprites;
    [SerializeField] private Image targetButton;
    [SerializeField] private Sprite[] otherbuttonBasicSprites;
    [SerializeField] private Sprite[] otherbuttonChangedSprites;
    [SerializeField] private Image[] otherButtons;
    private int ID;

    private void Start()
    {
        ID = PlayerPrefs.GetInt(key);
        if (key != "")
        {
            if (ID == 0)
                targetButton.sprite = buttonSprites[0];
            else
                targetButton.sprite = buttonSprites[1];
        }
     
    }

    public void ChangeSprite()
    {
        if (key != "")
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
        else
        {
            if (targetButton.sprite == buttonSprites[0])
            {
                targetButton.sprite = buttonSprites[1];
                return;
            }
            targetButton.sprite = buttonSprites[0];

        }
    }


    public void ChangeAllOtherSprites()
    {
     
        for (var i = 0; i < otherButtons.Length; i++)
            if (otherButtons[i].sprite == otherbuttonChangedSprites[i])
                otherButtons[i].sprite = otherbuttonBasicSprites[i];
    }
}