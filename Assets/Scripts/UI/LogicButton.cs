#region License
/*
* Copyright 2018 AutoCore
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
#endregion


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LogicButton : MonoBehaviour,IPointerDownHandler,IDragHandler,IBeginDragHandler
{
    public ElementObject obj;
    public void SetButtonObj(ElementObject elementObject)
    {
        obj = elementObject;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (obj != null && ElementsManager.Instance.editMode == ElementsManager.EditMode.Null&&eventData.button==PointerEventData.InputButton.Left)
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
