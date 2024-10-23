using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;

public class PopQuestionLoop : PopQuestion
{
    [SerializeField]
    DOTweenAnimation faderTween = null;

    protected override void OnWrongAnswer()
    {
        DisableSelectionMode();
        PlayWrongFeedbacks();

        enabled = false;
        faderTween.DOPlayForward();
        video.time = timeLock.GetStartTime();

        Invoke("LoopTime", faderTween.duration);
    }

    protected override void DisableSelectionMode()
    {
        selectionModeEnabled = false;
        timeDisplay.isOn = false;
        timeDisplay.currentPercent = 0;
        timeDisplay.UpdateUI();
        frameTween.DOPlayBackwards();
        selectionTarget.SetActive(false);
    }

    public override void OnRightAnswer()
    {
        base.OnRightAnswer();

        videoController.SetControlsEnabled(true);
        videoController.TogglePause(false);
    }

    void LoopTime()
    {
        wrongFeedback.gameObject.SetActive(false);

        video.time = timeLock.GetStartTime();
        StartCoroutine(WaitForVideoUpdate());
    }

    IEnumerator WaitForVideoUpdate()
    {
        while(video.time > timeLock.GetEndTime())
            yield return null;

        videoController.SetControlsEnabled(true);
        videoController.TogglePause(false);
        videoController.SetControlsEnabled(false);

        faderTween.DOPlayBackwards();
        enabled = true;
    }
}
