using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    void Start()
    {
        videoPlayer.GetComponent<VideoPlayer>();

        if(videoPlayer != null )
        videoPlayer.Play();
    }

    public void PlayVideoClip()
    {
        if (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }
}
