using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Michsky.MUIP;

public class ScoreManager : MonoBehaviour
{
    public float feedbackDelay = 1f;

    [SerializeField]
    ScoreBar scoreBar = null;

    [SerializeField]
    ButtonManager buttonManager = null;

    [SerializeField]
    List<BaseAnswerCheck> checks = new List<BaseAnswerCheck>();


    DOTweenAnimation visibleTween = null;

    bool visible = false;
    bool reviewStarted = false; 

    int score = 0;

    private void Start()
    {
        visibleTween = GetComponent<DOTweenAnimation>();

        scoreBar.Initialize(checks.Count, feedbackDelay);
    }

    private void Update()
    {
        for (int i = 0; i < checks.Count; i++)
        {
            if (checks[i].IsAnswered() == false)
            {
                if(visible)
                    HideCheckButton();

                return;
            }
        }

        if(!visible)
            ShowCheckButton();

    }

    void ShowCheckButton()
    {
        visibleTween.DOPlayForward();
        buttonManager.enabled = true;
        visible = true;
    }

    void HideCheckButton()
    {
        visibleTween.DOPlayBackwards();
        buttonManager.enabled = false;
        visible = false;
    }

    public void CheckAnswers()
    {
        if (reviewStarted == false)
        {
            StartCoroutine(CheckAnswersRoutine());
            reviewStarted = true;
        }
    }

    IEnumerator CheckAnswersRoutine()
    {
        for(int i = 0; i < checks.Count; i++)
        {
            bool rightAnswer = checks[i].CheckAnswer();

            if (rightAnswer)
            {
                score++;
                checks[i].RightAnswerFeedback();
                scoreBar.OnRightAnswer();
            }
            else
                checks[i].WrongAnswerFeedback();

            yield return new WaitForSeconds(feedbackDelay);
        }
    }
}
