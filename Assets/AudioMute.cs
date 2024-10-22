using UnityEngine;

public class AudioMute : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSource;

    private int soundID1;

    private void Start()
    {
        SetSoundVolume();
    }

    public void SetSoundVolume()
    {
        soundID1 = PlayerPrefs.GetInt("Sound");

        for (int i = 0; i < audioSource.Length; i++)
        {
            if (soundID1 == 0)
                audioSource[i].mute= false;
            else
                audioSource[i].mute= true;
        }
    }
}
