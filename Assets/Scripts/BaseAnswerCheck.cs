using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAnswerCheck : MonoBehaviour
{
    public abstract bool CheckAnswer();

    public abstract bool IsAnswered();

    public abstract void RightAnswerFeedback();

    public abstract void WrongAnswerFeedback();
}
