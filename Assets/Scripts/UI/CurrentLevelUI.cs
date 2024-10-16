using TMPro;
using UnityEngine;

public class CurrentLevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private PlatformGeneratorNew generatorNew;

    private void Update()
    {
        if (generatorNew != null) 
        text.text = "Level: " + generatorNew.Level.ToString();
    }
}
