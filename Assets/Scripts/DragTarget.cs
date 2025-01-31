using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

public class DragTarget : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    DOTweenAnimation visualizerTween = null;

    DraggableItem lastHoveringDraggable = null;
    DraggableItem attachedItem = null;

    [SerializeField]
    AudioSource onDragSuccess = null;

    protected override void Start()
    {
        visualizerTween = GetComponent<DOTweenAnimation>();

        Image visualizerImage = GetComponent<Image>();

        visualizerImage.color = new Color(visualizerImage.color.r, visualizerImage.color.g, visualizerImage.color.b, 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            DraggableItem currentHoveringDraggable = eventData.pointerDrag.GetComponent<DraggableItem>();

            if (currentHoveringDraggable && lastHoveringDraggable == null)
            {
                lastHoveringDraggable = currentHoveringDraggable;
                lastHoveringDraggable.SetDragTarget(this);

                visualizerTween.DOPlayForward();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DraggableItem currentHoveringDraggable = eventData.pointerDrag.GetComponent<DraggableItem>();

            if (currentHoveringDraggable && lastHoveringDraggable == currentHoveringDraggable)
            {
                lastHoveringDraggable.SetDragTarget(null);
                lastHoveringDraggable = null;

                if (attachedItem == currentHoveringDraggable)
                    attachedItem = null;

                visualizerTween.DOPlayBackwards();
            }
        }
    }

    public DraggableItem GetAttachedItem()
    {
        return attachedItem;
    }

    public void OnItemDragged(DraggableItem item)
    {
        if(attachedItem && attachedItem != item)
            attachedItem.ResetPosition();

        lastHoveringDraggable = null;

        visualizerTween.DOPlayBackwards();
        onDragSuccess.Play();
        attachedItem = item;
    }
}
