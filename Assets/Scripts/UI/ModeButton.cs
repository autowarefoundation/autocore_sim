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


using Assets.Scripts;
using AutoCore.Sim.Autoware.IO;
using System;
using UnityEngine.UI;

public class ModeButton : Button
{
    public Text text_modeName;
    public Text text_MapName;
    public Text text_LastTime;
    private SimuTestMode mode;
    private Button btn_edit;
    private Button btn_delete;
    public void SetModeButton(string modeName, string mapName, DateTime dateTime,SimuTestMode testmode)
    {
        if (text_modeName == null) text_modeName = transform.GetChild(0).GetComponent<Text>();
        text_modeName.text = modeName;
        if (text_MapName == null) text_MapName = transform.GetChild(1).GetComponent<Text>();
        text_MapName.text = mapName;
        if (text_LastTime == null) text_LastTime = transform.GetChild(2).GetComponent<Text>();
        text_LastTime.text = dateTime.ToString();
        mode = testmode;
        if (btn_edit == null) btn_edit = transform.GetChild(3).GetComponent<Button>();
        btn_edit.onClick.AddListener(()=> 
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
