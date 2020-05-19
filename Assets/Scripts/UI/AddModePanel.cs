﻿#region License
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
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AddModePanel : SimuPanel<AddModePanel>
{
    public Button button_addMode;
    public InputField inputField_modeName;
    public Dropdown dropDownItem;

    public Text text_NameNotice;
    private bool isNameLegal;
    private string modeName;
    void Start()
    {
        PanelInit();
    }
    public void PanelInit()
    {
        UpdateDropDownItem(Enum.GetNames(typeof(TestConfig.TestMap)));
        inputField_modeName?.onEndEdit.RemoveAllListeners();
        inputField_modeName?.onEndEdit.AddListener(CheckModeName);
        button_addMode.onClick.RemoveAllListeners();
        button_addMode?.onClick.AddListener(() =>
        {
            CheckModeName(inputField_modeName.text);
            if (isNameLegal)
            {
                TestDataManager.Instance.AddTestMode(inputField_modeName.text, Enum.GetNames(typeof(TestConfig.TestMap))[dropDownItem.value]);
                TestDataManager.Instance.WriteTestJson(true);
                TestConfig.isEditMode = true;
                SceneManager.LoadScene(TestConfig.TestMode.MapName+"_edit");
            }
        });
    }
    private void CheckModeName(string value)
    {
        isNameLegal = false;
        string pattern = @"^[A-Za-z0-9]+$";
        Regex regex = new Regex(pattern);
        if (!regex.IsMatch(value)) 
        {
            text_NameNotice.text = "Map name only can only consist of numbers and letters ";
            return; 
        }
        if (value == string.Empty)
        {
            text_NameNotice.text = "Map name cannot be empty";
            return;
        }
        foreach (var item in TestConfig.TestModes)
        {
            if (item.TestModeName == value)
            {
                text_NameNotice.text = "Map name cannot be repeated";
                return;
            }
        }
        isNameLegal = true;
        text_NameNotice.text = string.Empty;
    }

    void UpdateDropDownItem(string[] showNames)
    {
        dropDownItem.ClearOptions();
        Dropdown.OptionData temoData;

        for (int i = 0; i < showNames.Length; i++)
        {
            temoData = new Dropdown.OptionData();
            temoData.text = showNames[i];
            dropDownItem.options.Add(temoData);
        }
        dropDownItem.captionText.text = showNames[0];

    }
}
