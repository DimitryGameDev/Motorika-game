using UnityEngine;

public class AudioMute : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSource;

    private int soundID;

    private void Start()
    {
        SetSoundVolume();
    }

    public void SetSoundVolume()
    {
        soundID = PlayerPrefs.GetInt("Sound");

        for (int i = 0; i < audioSource.Length; i++)
        {
            if (soundID == 0)
                audioSource[i].mute= false;
            else
                audioSource[i].mute= true;
        }
    }
}
