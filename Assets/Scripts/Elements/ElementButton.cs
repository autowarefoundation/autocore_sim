#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion
using Assets.Scripts.Element;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.SimuUI
{

    public class ElementButton : Button
    {
        public GameObject elementObj;
        protected override void Start()
        {
            GetComponent<Button>()?.onClick.AddListener(delegate ()
            {
                if (ElementsManager.Instance.editMode == ElementsManager.EditMode.Null) ElementsManager.Instance.SelectedElement = elementObj.GetComponent<ElementObject>();
            });
        }
        protected override void OnDestroy()
        {
            if (elementObj != null) Destroy(elementObj);
        }
    }
}
