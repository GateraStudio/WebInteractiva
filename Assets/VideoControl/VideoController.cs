using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class VideoController : MonoBehaviour
{
    [SerializeField]
    VideoPlayer videoPlayer = null;

    [Header("Time Locks")]

    [SerializeField]
    List<VideoTimeLock> timeLocks = new List<VideoTimeLock>();

    [Header("Time")]

    [SerializeField]
    TextMeshProUGUI timeDisplay = null;

    [SerializeField]
    TextMeshProUGUI durationDisplay = null;

    [Header("Visibility")]

    [SerializeField]
    Slider controllerSlider = null;

    [SerializeField]
    DOTweenAnimation controllerFade = null;

    [SerializeField]
    float fadeOutTime = 2f;

    float fadeClock = 0f;
    float targetTimeValue = 0f;
    bool visibleControls = true;
    bool receivingInput = false;
    Vector3 lastMousePos = Vector2.zero;

    [Header("Pause")]

    [SerializeField]
    GameObject pauseButton = null;

    [SerializeField]
    DOTweenAnimation pauseTween = null;

    [Space]

    [SerializeField]
    GameObject playButton = null;

    [SerializeField]
    DOTweenAnimation playTween = null;

    [Space]

    [SerializeField]
    GameObject replayButton = null;

    [SerializeField]
    DOTweenAnimation replayTween = null;

    bool paused = false;
    bool replayShown = false;
    bool controlsEnabled = true;

    public bool IsPaused {  get { return paused; } }


    private void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        UpdateDurationDisplay();
    }


    double VideoDuration
    {
        get { return videoPlayer.frameCount / videoPlayer.frameRate; }
    }

    bool MouseMoved
    {
        get { return Input.mousePosition != lastMousePos; }
    }

    public void SetControlsEnabled(bool value)
    {
        controlsEnabled = value;
    }

    void ShowControls()
    {
        controllerFade.DOPlayBackwards();
        visibleControls = true;
        fadeClock = 0f;
    }

    void HideControls()
    {
        controllerFade.DOPlayForward();
        visibleControls = false;
    }

    void OnVideoEnd(VideoPlayer source)
    {
        playButton.SetActive(false);
        pauseButton.SetActive(false);
        replayButton.SetActive(true);

        replayShown = true;
    }

    void Update()
    {
        UpdateTimeDisplay();

        if(visibleControls)
        {
            if (!MouseMoved && !receivingInput)
                fadeClock += Time.deltaTime;
            else
                fadeClock = 0f;

            if(fadeClock > fadeOutTime)
                HideControls();
        }
        else
        {
            if(MouseMoved || receivingInput)
                ShowControls();
        }


        if(receivingInput)
        {
            if (Input.GetMouseButton(0) == false && paused == false)
            {
                receivingInput = false;
                videoPlayer.Play();
            }
        }
        else
            controllerSlider.value = targetTimeValue = (float)(videoPlayer.time / VideoDuration);

        lastMousePos = Input.mousePosition;
    }

    void UpdateTimeDisplay()
    {
        TimeSpan time = TimeSpan.FromSeconds(videoPlayer.time); 
        timeDisplay.text = time.ToString("m':'ss");
    }

    void UpdateDurationDisplay()
    {
        TimeSpan time = TimeSpan.FromSeconds(videoPlayer.length);
        durationDisplay.text = "/ " + time.ToString("m':'ss");
    }

    public void OnSliderValueChanged()
    {
        if (!controlsEnabled)
            return;

        if (controllerSlider.value != targetTimeValue)
        {
            for(int i = 0; i < timeLocks.Count; i++)
            {
                if(timeLocks[i].IsTargetTimeLocked(controllerSlider.value * (float)VideoDuration) && timeLocks[i].isActiveAndEnabled)
                {
                    timeLocks[i].PlayLockTween();
                    controllerSlider.value = targetTimeValue = (float)(videoPlayer.time / VideoDuration);
                    return;
                }
            }

            if (replayShown)
                Replay(false);

            receivingInput = true;
            videoPlayer.Pause();
            videoPlayer.time = controllerSlider.value * VideoDuration;
        }
    }

    public void Pause(bool showTween)
    {
        if (!paused)
            TogglePause(showTween);
    }

    public void Unpause(bool showTween)
    {
        if (paused)
            TogglePause(showTween);
    }

    public void TogglePause(bool showTween)
    {
        if (!controlsEnabled)
            return;

        ShowControls();

        if(replayShown)
        {
            Replay(showTween);
            return;
        }

        paused = !paused;

        if(paused)  videoPlayer.Pause();
        else        videoPlayer.Play();

        pauseButton.SetActive(!paused);
        playButton.SetActive(paused);

        if(showTween)
        {
            if(paused)
            {
                pauseTween.DORewindAllById("PauseTween");
                pauseTween.DOPlayAllById("PauseTween");
            }
            else
            {
                playTween.DORewindAllById("PlayTween");
                playTween.DOPlayAllById("PlayTween");
            }
        }
    }

    void Replay(bool showTween)
    {
        pauseButton.SetActive(true);
        playButton.SetActive(false);
        replayButton.SetActive(false);

        paused = false;
        replayShown = false;

        videoPlayer.time = 0;
        videoPlayer.Play();

        if (showTween)
        {
            replayTween.DORewindAllById("ReplayTween");
            replayTween.DOPlayAllById("ReplayTween");
        }
    }
}
