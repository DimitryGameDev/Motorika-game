using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    private VideoPlayer video;
    void Start()
    {
        video.GetComponent<VideoPlayer>();

        if(video != null )
        video.Play();
    }

    public void PlayVideoClip()
    {
        if (!video.isPlaying)
        {
            video.Play();
        }
    }
}
