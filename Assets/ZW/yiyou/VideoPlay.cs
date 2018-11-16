using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlay : SingletonMono<VideoPlay>
{
    public Button videoPlayButton;
    //Raw Image to Show Video Images [Assign from the Editor]
    public RawImage image;
    //Video To Play [Assign from the Editor]


    private VideoPlayer videoPlayer;
    private VideoSource videoSource;

    //Audio
    private AudioSource audioSource;
    private string videoPath;

    public void Init(string videoPath)
    {
        image = GetComponent<RawImage>();

        videoPlayButton = transform.Find("PlayButton").GetComponent<Button>();
        videoPlayButton.gameObject.SetActive(true);
        videoPlayButton.onClick.AddListener(Play);
        this.videoPath = videoPath;
        StartCoroutine(playVideo());
    }
    public void Play()
    {
        //  Play Video
        videoPlayer.Play();

        //    Play Sound
        audioSource.Play();


        videoPlayButton.gameObject.SetActive(false);
    }

    IEnumerator playVideo()
    {
        //Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = gameObject.GetComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        videoPlayer.isLooping = false;
        audioSource.loop = false;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = "file://" + videoPath;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.loopPointReached += EndReached;

        //Google到的解决方案
        //这里一定要让以上工作完成后才能开始准备videoPlayer  并且赋值视频输出Texture
        videoPlayer.Prepare();

        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
         
            yield return null;
        }

        //Set Raw Image to Show Video Images
        image.texture = videoPlayer.texture;
        image.color = new UnityEngine.Color(1,1,1,1);
        //   PlayVideo();


        //Debug.Log("Playing Video");
        while (videoPlayer.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }

        Debug.Log("Done Playing Video");
    }
    /// <summary>
    /// 当VideoPlayer到达要播放的内容的结尾时调用。
  /// 如果loop属性设置为true，则会发生循环。否则VideoPlayer将停止。
    /// </summary>
    /// <param name="vp"></param>
    private void EndReached(VideoPlayer vp)
    {
        
        videoPlayButton.gameObject.SetActive(true);
    }
}
