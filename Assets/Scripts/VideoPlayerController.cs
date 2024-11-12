using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer VideoPlayer { get; private set; }

    private void Awake()
    {
        if (VideoPlayer == null)
            VideoPlayer = GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (VideoPlayer.isPlaying)
                VideoPlayer.Pause();

            else
                VideoPlayer.Play();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
            VideoPlayer.Stop();
    }

    public void PlayVideo(VideoPlayer vp)
    {
        vp.Play();
    }
}