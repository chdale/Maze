using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MazeIntroVideo : MonoBehaviour {

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndVideo;
	    if (GlobalStats.FirstRun)
        {
            StartCoroutine(PlayVideo());
        }	
	}

    private void EndVideo(VideoPlayer source)
    {
        source.enabled = false;
    }

    private IEnumerator PlayVideo()
    {
        yield return new WaitForSeconds(2f);
        videoPlayer.Play();
    }
}
