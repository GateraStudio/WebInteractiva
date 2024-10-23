using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VideoTimeLock : MonoBehaviour
{
    [SerializeField]
    float startTime = 0;

    [SerializeField]
    float endTime = 0;

    [Header("Display")]

    [SerializeField]
    DOTweenAnimation lockTween = null;

    bool lockActive = true;

    public float GetStartTime()
    {
        return startTime;
    }

    public float GetEndTime()
    {
        return endTime;
    }


    public bool IsTargetTimeLocked(float targetTime)
    {
        return lockActive && targetTime > startTime;
    }

    public void PlayLockTween()
    {
        lockTween.DORestart();
        lockTween.DOPlay();
    }
}
