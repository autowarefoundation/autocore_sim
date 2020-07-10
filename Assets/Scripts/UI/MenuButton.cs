using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class MenuButton : Button
    {
        Text Text_name
        {
            get
            {
                return GetComponentInChildren<Text>();
            }
        }
        public Transform Transform_menu
        {
            get
            {
                return transform.GetChild(1);
            }
        }
        public bool isOpen = false;
        protected override void Start()
        {
            base.Start();
        }
        public void SetMenu(string name, UnityAction method)
        {
            Text_name.text = name;
            onClick.AddListener(method);
        }
        public void AddMunuButton(string name, UnityAction method, GameObject ToolButtonLV2)
        {
            GameObject TB = Instantiate(ToolButtonLV2);
            TB.transform.SetParent(Transform_menu);
            TB.GetComponentInChildren<Text>().text = name;
            TB.GetComponent<Button>().onClick.AddListener(method);
            TB.GetComponent<Button>().onClick.AddListener(() => { PanelTools.Instance.CloseAllMenu(); });

        }
        public void SetMenuActive(bool isActive)
        {
            Transform_menu.gameObject.SetActive(isActive);
            isOpen = isActive;
        }
    }
}
