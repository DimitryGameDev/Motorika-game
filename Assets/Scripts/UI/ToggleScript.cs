using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private Sprite[] buttonSprites;
    [SerializeField] private Image targetButton;
    [SerializeField] private Image onStartClickedButton;
    [SerializeField] private Sprite[] otherbuttonBasicSprites;
    [SerializeField] private Sprite[] otherbuttonChangedSprites;
    [SerializeField] private Image[] otherButtons;
    private int ID;

    private void Start()
    {
        SetSprite();
    }

    public void SetSprite()
    {
        ID = PlayerPrefs.GetInt(key);
        if (key != "")
        {
            if (ID == 0)
                targetButton.sprite = buttonSprites[0];
            else
                targetButton.sprite = buttonSprites[1];
        }

        if (onStartClickedButton)
        {
            onStartClickedButton.sprite = buttonSprites[1];
        }
    }

    public void ChangeSprite()
    {
        if (key != "")
        {
            if (key == "Sound")
            {
                if (targetButton.sprite == buttonSprites[0])
                {
                    targetButton.sprite = buttonSprites[1];
                    PlayerPrefs.SetInt("Sound", 1);
                }
                else
                {
                    targetButton.sprite = buttonSprites[0];

                    PlayerPrefs.SetInt("Sound", 0);
                }

                PlayerPrefs.Save();
            }
            
            if (key == "Control")
            {
                if (targetButton.sprite == buttonSprites[0])
                {
                    targetButton.sprite = buttonSprites[1];
                    PlayerPrefs.SetInt("Control", 1);
                }
                else
                {
                    targetButton.sprite = buttonSprites[0];

                    PlayerPrefs.SetInt("Control", 0);
                }

                PlayerPrefs.Save();
            }
        }
        else
        {
            if (targetButton.sprite == buttonSprites[0])
            {
                targetButton.sprite = buttonSprites[1];
               
            }
        }
    }
    
    public void ChangeAllOtherSprites()
    {
        for (var i = 0; i < otherButtons.Length; i++)
            if (otherButtons[i].sprite == otherbuttonChangedSprites[i])
                otherButtons[i].sprite = otherbuttonBasicSprites[i];
    }
}