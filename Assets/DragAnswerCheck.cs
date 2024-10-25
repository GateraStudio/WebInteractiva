using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class DragAnswerCheck : BaseAnswerCheck
{
    [SerializeField]
    public List<DraggableItem> rightAnswers = new List<DraggableItem>();

    [FoldoutGroup("Feedbacks", expanded: false)]
    [SerializeField]
    UnityEvent RightFeedback = new UnityEvent();

    [FoldoutGroup("Feedbacks", expanded: false)]
    [SerializeField]
    UnityEvent WrongFeedback = new UnityEvent();

    DragTarget target = null;

    void Awake()
    {
        target = GetComponentInChildren<DragTarget>();
    }

    public override bool CheckAnswer()
    {
        for(int i = 0; i < rightAnswers.Count; i++)
        {
            if(rightAnswers[i] == target.GetAttachedItem())
                return true;
        }
        return false;
    }

    public override void RightAnswerFeedback()
    {
        RightFeedback.Invoke();
        target.GetAttachedItem().PlayRightFeedback();
    }

    public override void WrongAnswerFeedback()
    {
        WrongFeedback.Invoke();
        target.GetAttachedItem().PlayWrongFeedback();
    }

    public override bool IsAnswered()
    {
        return target.GetAttachedItem() != null;
    }
}
