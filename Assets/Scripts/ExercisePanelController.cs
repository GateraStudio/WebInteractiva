using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.MUIP;
using UnityEngine.Events;

public class ExercisePanelController : MonoBehaviour
{
    [SerializeField]
    VideoController videoController = null;

    [SerializeField]
    UnityEvent onOpen = new UnityEvent();

    [SerializeField]
    UnityEvent onClose = new UnityEvent();

    [SerializeField]
    UnityEvent onLock = new UnityEvent();

    bool panelOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (!videoController.IsPaused && panelOpen)
            ClosePanel();
    }

    public void OpenPanel()
    {
        if (!panelOpen)
        {
            onOpen.Invoke();
            panelOpen = true;
        }
    }

    public void ClosePanel()
    {
        if (panelOpen)
        {
            onClose.Invoke();
            panelOpen = false;
        }
    }

    public void LockPanel()
    {
        if (panelOpen)
        {
            onClose.Invoke();
            panelOpen = false;
        }

        onLock.Invoke();
    }
}
