using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class DraggableItem : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    RectTransform dragArea;

    [SerializeField]
    bool topOnDrag = true;

    [Title("Audio")]


    [SerializeField]
    AudioSource onDragFailure = null;

    [Title("Feedbacks")]

    [FoldoutGroup("Feedbacks", expanded: false)]
    [SerializeField]
    UnityEvent RightFeedback = new UnityEvent();

    [FoldoutGroup("Feedbacks", expanded: false)]
    [SerializeField]
    UnityEvent WrongFeedback = new UnityEvent();

    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;

    Vector2 startingPos = Vector2.zero;
    DragTarget dragTarget = null;

    CanvasGroup canvasGroup = null;

    protected override void Start()
    {
        startingPos = rectTransform.localPosition;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private RectTransform rectTransform
    {
        get { return (transform as RectTransform); }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        originalPanelLocalPosition = rectTransform.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(dragArea, data.position, data.pressEventCamera, out originalLocalPointerPosition);
        canvasGroup.blocksRaycasts = false;
        gameObject.transform.SetAsLastSibling();
        if (topOnDrag == true) { rectTransform.transform.SetAsLastSibling(); }
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 localPointerPosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(dragArea, data.position, data.pressEventCamera, out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
            rectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;
        }

        ClampToArea();
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (dragTarget)
        {
            rectTransform.position = dragTarget.transform.position;
            dragTarget.OnItemDragged(this);
        }
        else
        {
            onDragFailure.Play();
            ResetPosition();
        }

        canvasGroup.blocksRaycasts = true;

    }

    public void ResetPosition()
    {
        rectTransform.localPosition = startingPos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && dragTarget)
        {
            dragTarget.OnPointerEnter(eventData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && dragTarget)
        {
            dragTarget?.OnPointerExit(eventData);
        }
    }

    public void SetDragTarget(DragTarget target)
    {
        dragTarget = target;
    }

    private void ClampToArea()
    {
        Vector3 pos = rectTransform.localPosition;

        Vector3 minPosition = dragArea.rect.min - rectTransform.rect.min;
        Vector3 maxPosition = dragArea.rect.max - rectTransform.rect.max;

        pos.x = Mathf.Clamp(rectTransform.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(rectTransform.localPosition.y, minPosition.y, maxPosition.y);

        rectTransform.localPosition = pos;
    }

    public void PlayRightFeedback()
    {
        RightFeedback.Invoke();
    }

    public void PlayWrongFeedback()
    {
        WrongFeedback.Invoke();
    }
}

