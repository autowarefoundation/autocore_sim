#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion

using AutoCore.Sim.Autoware.IO;
using System;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class ModeButton : Button
    {
        public Text text_modeName;
        public Text text_MapName;
        public Text text_LastTime;
        private SimuTestMode mode;
        private Button btn_edit;
        private Button btn_delete;
        public void SetModeButton(string modeName, string mapName, DateTime dateTime, SimuTestMode testmode)
        {
            if (text_modeName == null) text_modeName = transform.GetChild(0).GetComponent<Text>();
            text_modeName.text = modeName;
            if (text_MapName == null) text_MapName = transform.GetChild(1).GetComponent<Text>();
            text_MapName.text = mapName;
            if (text_LastTime == null) text_LastTime = transform.GetChild(2).GetComponent<Text>();
            text_LastTime.text = dateTime.ToString();
            mode = testmode;
            if (btn_edit == null) btn_edit = transform.GetChild(3).GetComponent<Button>();
            btn_edit.onClick.AddListener(() =>
            {
                TestConfig.TestMode = mode;
                TestConfig.isEditMode = true;
                LunchUI.Instance.EnterSimu(true);
            });
            if (btn_delete == null) btn_delete = transform.GetChild(4).GetComponent<Button>();
            btn_delete.onClick.AddListener(() =>
            {
                TestConfig.DeleteData(mode);
                gameObject.SetActive(false);
            });

            onClick.RemoveAllListeners();
            onClick.AddListener(() =>
            {
                TestConfig.TestMode = mode;
                TestConfig.isEditMode = true;
                if (ROS_Config.ROS2)
                {
                    LunchUI.Instance.EnterSimu(false);
                }
                else
                    LunchUI.Instance.TestConnect(mode);
            });
        }
    }
}
