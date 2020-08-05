using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    [RequireComponent(typeof(Button))]
    public class MenuLV1 : MonoBehaviour
    {
        public Button button;
        public GameObject targetGO;
        public GameObject TargetGO
        {
            get
            {
                if(targetGO==null)
                    targetGO = transform.GetChild(1).gameObject;
                return targetGO;
            }
        }
        public bool isOpen;
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                PanelTools.Instance.MenuButtonSelected = this;
            });
        }
        public void SetMenuActive(bool isActive)
        {
            isOpen = isActive;
            var panel = TargetGO.GetComponent<ISimuPanel>();
            if (panel != null)
            {
                panel.SetPanelActive(isOpen);
            }
            else
            {
                TargetGO.SetActive(isOpen);
            }
        }

    }
}
