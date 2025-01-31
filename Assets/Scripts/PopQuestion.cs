using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Michsky.MUIP;
using DG.Tweening;

public class PopQuestion : MonoBehaviour
{
    [SerializeField]
    protected VideoPlayer video = null;

    [SerializeField]
    protected VideoController videoController = null;

    [SerializeField]
    protected ProgressBar timeDisplay = null;

    [SerializeField]
    protected VideoTimeLock timeLock = null;

    [SerializeField]
    protected DOTweenAnimation displayTween = null;

    [SerializeField]
    protected DOTweenAnimation frameTween = null;

    [SerializeField]
    protected GameObject selectionTarget = null;

    [SerializeField]
    protected DOTweenAnimation wrongFeedback = null;

    [SerializeField]
    protected DOTweenAnimation rightFeedback = null;

    [SerializeField]
    protected ExercisePanelController exercisePanelController = null;

    [SerializeField]
    float answerTime = 5f;

    bool displayed = false;
    protected bool selectionModeEnabled = false;

    private void Start()
    {
        timeDisplay.maxValue = answerTime;
    }

    void Update()
    {
        if(video.time > timeLock.GetStartTime() && !displayed)
            ShowQuestion();

        else if(video.time < timeLock.GetStartTime() && displayed)
            HideQuestion();

        if (video.time > timeLock.GetEndTime() && !selectionModeEnabled)
            EnableSelectionMode();

        if ((timeDisplay.currentPercent / timeDisplay.maxValue) >= 1f && selectionModeEnabled)
            OnWrongAnswer();
        
    }

    public virtual void OnRightAnswer()
    {
        DisableSelectionMode();
        HideQuestion();
        PlayRightFeedbacks();

        timeLock.gameObject.SetActive(false);
        enabled = false;
    }

    protected virtual void OnWrongAnswer()
    {
        DisableSelectionMode();
        HideQuestion();
        PlayWrongFeedbacks();

        timeLock.gameObject.SetActive(false);
        enabled = false;
    }

    protected void EnableSelectionMode()
    {
        videoController.SetControlsEnabled(true);
        videoController.TogglePause(false);
        videoController.SetControlsEnabled(false);
        selectionModeEnabled = true;
        timeDisplay.isOn = true;
        timeDisplay.currentPercent = 0;
        frameTween.DOPlayForward();
        selectionTarget.SetActive(true);
    }

    protected virtual void DisableSelectionMode()
    {
        videoController.SetControlsEnabled(true);
        selectionModeEnabled = false;
        timeDisplay.isOn = false;
        timeDisplay.currentPercent = 0;
        timeDisplay.UpdateUI();
        frameTween.DOPlayBackwards();
        selectionTarget.SetActive(false);
        videoController.TogglePause(false);
    }

    protected void PlayWrongFeedbacks()
    {
        wrongFeedback.gameObject.SetActive(true);
        wrongFeedback.DORestart();
        wrongFeedback.DOPlay();
    }

    protected void PlayRightFeedbacks()
    {
        rightFeedback.gameObject.SetActive(true);
        rightFeedback.DORestart();
        rightFeedback.DOPlay();
    }


    protected void ShowQuestion()
    {
        displayTween.DOPlayForward();
        exercisePanelController.LockPanel();
        displayed = true;
    }

    protected void HideQuestion()
    {
        displayTween.DOPlayBackwards();
        displayed = false;
    }
}
