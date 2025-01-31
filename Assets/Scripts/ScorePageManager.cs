using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class ScorePageManager : MonoBehaviour
{
    [SerializeField]
    AudioSource starSFX = null;

    [SerializeField]
    List<TextMeshProUGUI> scoreTexts = new List<TextMeshProUGUI>();

    [SerializeField]
    List<DOTweenAnimation> stars = new List<DOTweenAnimation>();

    int obtainedStars = 0;
    int tweenedStars = 0;

    public void RightAnswerOnExercise(int exerciseIndex)
    {
        if(exerciseIndex >= 0 && exerciseIndex < scoreTexts.Count)
        {
            string prevText = scoreTexts[exerciseIndex].text;

            int value = 0;
            int.TryParse(prevText.Substring(0, 1), out value);

            value++;

            scoreTexts[exerciseIndex].text = value.ToString() + prevText.Substring(1, prevText.Length - 1);
        }
    }

    public void StarObtained()
    {
        obtainedStars++;
    }

    public void PlayStarTweens()
    {
        StopAllCoroutines();
        StartCoroutine(StarTweensCoroutine());
    }

    IEnumerator StarTweensCoroutine()
    {
        for (int i = tweenedStars; i < Mathf.Min(stars.Count, obtainedStars); i++)
        {
            tweenedStars++;
            stars[i].DOPlay();
            
            if(starSFX)
                starSFX.Play(); 

            yield return new WaitForSeconds(0.6f);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
