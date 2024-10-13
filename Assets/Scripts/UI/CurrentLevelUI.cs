using UnityEngine;
using UnityEngine.UI;

public class CurrentLevelUI : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private PlatformGeneratorNew generatorNew;

    private void Update()
    {
        if (generatorNew != null) 
        text.text = generatorNew.Level.ToString();
    }
}
