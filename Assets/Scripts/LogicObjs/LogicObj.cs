#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LogicObj : MonoBehaviour
    {
        public ElementObject elementObject;
        public LogicButton logicButton;
        public virtual void Start()
        {
            if (logicButton == null) logicButton = GetComponentInChildren<LogicButton>();
            logicButton.SetButtonObj(elementObject);
        }
    }
}
