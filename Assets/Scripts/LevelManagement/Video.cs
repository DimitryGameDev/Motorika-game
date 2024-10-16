using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    private VideoPlayer Player;
    void Start()
    {
        Player.GetComponent<VideoPlayer>();

        if(Player != null )
        Player.Play();
    }

    public void PlayVideoClip()
    {
        if (!Player.isPlaying)
        {
            Player.Play();
        }
    }
}
