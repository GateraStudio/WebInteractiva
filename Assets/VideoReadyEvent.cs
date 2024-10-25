using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

[RequireComponent(typeof(VideoPlayer))]
public class VideoReadyEvent : MonoBehaviour
{
    VideoPlayer player = null;

    [SerializeField]
    UnityEvent onVideoReady = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        player.EnableAudioTrack(0, true);
        player.Prepare();
        player.prepareCompleted += Prepared;
        player.Play();
    }

    // Update is called once per frame
    void Prepared(VideoPlayer source)
    {
        if (player.isPrepared)
        {
            onVideoReady.Invoke();
            enabled = false;
        }
    }
}
