#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion
using Assets.Scripts.Element;
using UnityEngine;
using UnityEngine.EventSystems;


public class LogicButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler
{
    public ElementObject obj;
    public void SetButtonObj(ElementObject elementObject)
    {
        obj = elementObject;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (obj != null && ElementsManager.Instance.editMode == ElementsManager.EditMode.Null && eventData.button == PointerEventData.InputButton.Left)
        {
            ElementsManager.Instance.SelectedElement = obj;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (obj != null && ElementsManager.Instance.editMode == ElementsManager.EditMode.Null && eventData.button == PointerEventData.InputButton.Left)
        {
            obj.StartDrag();
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (obj != null && ElementsManager.Instance.editMode == ElementsManager.EditMode.Null && eventData.button == PointerEventData.InputButton.Left)
        {
            obj.ObjDrag();
        }
    }
    private void OnDestroy()
    {
        if (obj != null)
        {
            Destroy(obj.gameObject);
        }
    }
}
