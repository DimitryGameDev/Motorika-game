using UnityEngine;

public class AudioMute : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSource;

    private int soundID2;

    private void Start()
    {
        SetSoundVolume();
    }

    public void SetSoundVolume()
    {
        soundID2 = PlayerPrefs.GetInt("Sound");

        for (int i = 0; i < audioSource.Length; i++)
        {
            if (soundID2 == 0)
                audioSource[i].mute= false;
            else
                audioSource[i].mute= true;
        }
    }
}
