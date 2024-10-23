using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;
using Michsky.MUIP;

public class ScoreBar : MonoBehaviour
{
    [SerializeField]
    int scorePerAnswer = 1000;

    int maxAnswers = 0;

    [Title("Score Text")]
    [SerializeField]
    TextMeshProUGUI scoreText = null;

    [MinMaxSlider(minValue: 40f, maxValue: 100, showFields: true)]
    [SerializeField]
    Vector2 minMaxfontSize = new Vector2(40f, 80f);

    [SerializeField]
    [HorizontalGroup("Stars", Title = "Stars")]
    List<DOTweenAnimation> starTweens = new List<DOTweenAnimation>();

    [HorizontalGroup("Stars", Title = "Stars")]
    [SerializeField]
    List<int> starScores = new List<int>();

    [Space]
    [SerializeField]
    UnityEvent onStarObtained = new UnityEvent();

    ProgressBar progressBar = null;
    int targetScore = 0;
    float currentScore = 0;
    float lerpCoroutineTime = 0f;
    float startingTextY = 0f;

    public void Initialize(int _maxAnswers, float feedbackDelay)
    {
        maxAnswers = _maxAnswers;

        progressBar = GetComponent<ProgressBar>();
        progressBar.maxValue = maxAnswers * scorePerAnswer;

        lerpCoroutineTime = feedbackDelay * 0.8f;
        startingTextY = scoreText.transform.localPosition.y;
    }

    public void OnRightAnswer()
    {
        targetScore = (int)currentScore + scorePerAnswer;
        StopAllCoroutines();
        StartCoroutine(LerpToValueCoroutine());
    }

    void UpdateText()
    {
        scoreText.fontSize = Mathf.Lerp(minMaxfontSize.x, minMaxfontSize.y, currentScore / (maxAnswers * scorePerAnswer));

        RectTransform rectTransform = transform as RectTransform;

        float targetY = rectTransform.rect.size.x * (currentScore / (maxAnswers * scorePerAnswer));
        scoreText.transform.localPosition = new Vector3(scoreText.transform.localPosition.x, startingTextY + targetY, 0f);
    }

    IEnumerator LerpToValueCoroutine()
    {
        float clock = 0f;
        float lastScore = currentScore;

        while(clock < lerpCoroutineTime)
        {
            clock += Time.deltaTime;

            currentScore = (int)Mathf.Lerp(lastScore, targetScore, clock / lerpCoroutineTime);
            progressBar.currentPercent = currentScore;
            progressBar.UpdateUI();

            UpdateText();

            yield return null;
        }

        progressBar.currentPercent = currentScore = targetScore;
        UpdateText();
        progressBar.UpdateUI();

        for (int i = 0; i < starScores.Count; i++)
        {
            if (currentScore == starScores[i])
            {
                starTweens[i].DOPlay();
                onStarObtained.Invoke();
            }
        }
    }
}
