using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class GameMenuManager : MonoBehaviour
{
    [HorizontalGroup("DisplacementControl", Title = "Displacement Control")]
    [SerializeField]
    List<DOTweenAnimation> icons = new List<DOTweenAnimation>();

    [HorizontalGroup("DisplacementControl", Title = "Displacement Control")]
    [SerializeField]
    List<DOTweenAnimation> displacements = new List<DOTweenAnimation>();

    int currentlySelected = 0;

    // Start is called before the first frame update
    void Start()
    {
        icons[currentlySelected].DOPlayForward();
    }

    public void SelectNext()
    {
        if(currentlySelected < icons.Count - 1)
        {
            icons[currentlySelected].DOPlayBackwards();

            currentlySelected++;

            icons[currentlySelected].DOPlayForward();
            displacements[currentlySelected].RecreateTween();
            displacements[currentlySelected].DOPlay();

        }
    }

    public void SelectPrev()
    {
        if (currentlySelected > 0)
        {
            icons[currentlySelected].DOPlayBackwards();

            currentlySelected--;

            icons[currentlySelected].DOPlayForward();
            displacements[currentlySelected].RecreateTween();
            displacements[currentlySelected].DOPlay();
        }
    }

}
