using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TFAnswerCheck : BaseAnswerCheck
{
    [SerializeField]
    public bool answerIsTrue = false;

    [FoldoutGroup("Feedbacks", expanded: false)]
    [SerializeField]
    UnityEvent RightFeedback = new UnityEvent();

    [FoldoutGroup("Feedbacks", expanded: false)]
    [SerializeField]
    UnityEvent WrongFeedback = new UnityEvent();

    Toggle target = null;

    void Awake()
    {
        target = GetComponent<Toggle>();
    }

    public override bool CheckAnswer()
    {
        return (target.isOn && answerIsTrue) || (!target.isOn && !answerIsTrue);
    }

    public override void RightAnswerFeedback()
    {
        RightFeedback.Invoke();
    }

    public override void WrongAnswerFeedback()
    {
        WrongFeedback.Invoke();
    }

    public override bool IsAnswered()
    {
        return true;
    }
}
