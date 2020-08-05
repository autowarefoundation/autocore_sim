using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.SimuUI
{
    public class PanelBase<T> : MonoBehaviour where T : PanelBase<T>
    {
        private static T _instance = null;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(T)) as T;
                }
                if (_instance == null)
                {
                    Debug.Log("null");
                }
                return _instance;
            }
        }
        protected virtual void Awake()
        {
            _instance = (T)this;
            SetPanelActive(isActive);
        }
        public bool isActive;
        public virtual void SwitchPanelActive()
        {
            isActive = !isActive;
            SetPanelActive(isActive);
        }
        public virtual void SetPanelActive(bool value)
        {
            isActive = value;
            if (isActive)
            {
                MainUI.Instance.AddPanel((ISimuPanel)this);
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                MainUI.Instance.RemovePanel((ISimuPanel)this);
                transform.localScale = Vector3.zero;
            }
        }
    }

}