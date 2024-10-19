using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioClick : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    private AudioSource audioSource;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        for (int i =0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(OnClicked);
        }
    }

    private void OnDestroy()
    {
        for (int i =0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveListener(OnClicked);
        }
    }

    private void OnClicked()
    {
        if(audioSource != null)
        audioSource.PlayOneShot(audioSource.clip);
    }
}
