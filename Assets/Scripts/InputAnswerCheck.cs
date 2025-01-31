using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InputAnswerCheck : BaseAnswerCheck
{
    [SerializeField]
    public string answer = "";

    [FoldoutGroup("Feedbacks", expanded: false)]
    [SerializeField]
    UnityEvent RightFeedback = new UnityEvent();

    [FoldoutGroup("Feedbacks", expanded: false)]
    [SerializeField]
    UnityEvent WrongFeedback = new UnityEvent();

    TMP_InputField input = null;

    void Awake()
    {
        input = GetComponent<TMP_InputField>();
    }

    public override bool CheckAnswer()
    {
        return input.text == answer;
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
        return input.text.Length > 0;
    }
}
