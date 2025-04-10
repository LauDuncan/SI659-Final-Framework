using UnityEngine;
using UnityEngine.Video;

public class SetVideoAudio : MonoBehaviour
{
    public VideoPlayer videoPlayer;          // 通过 Inspector 分配 VideoPlayer
    public AudioSource yourAudioSource;      // 通过 Inspector 分配目标 AudioSource

    void Start()
    {
        // 将 VideoPlayer 的音频输出模式设置为 AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        // 将 Track 0 的音频输出指定给你的 AudioSource
        videoPlayer.SetTargetAudioSource(0, yourAudioSource);
    }
}
