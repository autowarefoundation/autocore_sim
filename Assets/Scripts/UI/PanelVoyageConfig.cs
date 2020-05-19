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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PanelVoyageConfig : MonoBehaviour
{
    public GameObject Indicator;
    public Text text_Title;
    public InputField inputField_TargetSpeed;
    public InputField inputField_MinSpeed;
    public InputField inputField_MaxSpeed;
    public InputField inputField_MinDis;
    public InputField inputField_MaxDis;
    public InputField inputField_Duration;
    public Button button_Delete;
    private VoyageConfig voyageConfig;
    private int index_config;
    // Start is called before the first frame update
    void Start()
    {
        inputField_TargetSpeed?.onEndEdit.AddListener((string str)=> 
        {
            float value = float.Parse(str);
            value = Mathf.Clamp(value, 0, 100);
            voyageConfig.targetSpeed = value;
            if (index_config == VoyageTestManager.Instance.IndexTest)
            {
                VoyageTestManager.Instance.UpdateTargetSpeed();
            }
        });
        inputField_MinSpeed?.onEndEdit.AddListener((string str) =>
        {
            float value = float.Parse(str);
            value = Mathf.Clamp(value, 0, 100);
            voyageConfig.testCarMinSpeed = value;
        });
        inputField_MaxSpeed?.onEndEdit.AddListener((string str) =>
        {

            float value = float.Parse(str);
            value = Mathf.Clamp(value, 0, 100);
            voyageConfig.testCarMaxSpeed = value;
        });
        inputField_MinDis?.onEndEdit.AddListener((string str) =>
        {

            float value = float.Parse(str);
            value = Mathf.Clamp(value, 0, 100);
            voyageConfig.minDis2Target = value;
        });
        inputField_MaxDis?.onEndEdit.AddListener((string str) =>
        {
            float value = float.Parse(str);
            value = Mathf.Clamp(value, 0, 100);
            voyageConfig.maxDis2Target = value;

        });
        inputField_Duration?.onEndEdit.AddListener((string str) =>
        {
            float value = float.Parse(str);
            value = Mathf.Clamp(value, 0, 100);
            voyageConfig.duration = value;
        });
        button_Delete?.onClick.AddListener(()=> 
        {
            if (voyageConfig != null && PanelVoyage.Instance.panelVoyageConfigs.Contains(this))
            {
                RemoveVoyageConfig();
            }
        });
    }
    public void InitPanel(VoyageConfig config)
    {
        voyageConfig = config;
        if (!VoyageTestManager.Instance.VoyageConfigs.Contains(config)) VoyageTestManager.Instance.VoyageConfigs.Add(config);
        index_config = VoyageTestManager.Instance.VoyageConfigs.IndexOf(config);

        text_Title.text = "Step:" + index_config;
        inputField_TargetSpeed.text = voyageConfig.targetSpeed.ToString();
        inputField_MinSpeed.text = voyageConfig.testCarMinSpeed.ToString();
        inputField_MaxSpeed.text = voyageConfig.testCarMaxSpeed.ToString();
        inputField_MinDis.text = voyageConfig.minDis2Target.ToString();
        inputField_MaxDis.text = voyageConfig.maxDis2Target.ToString();
        inputField_Duration.text = voyageConfig.duration.ToString();
    }
    public void RemoveVoyageConfig()
    {
        VoyageTestManager.Instance.VoyageConfigs.Remove(voyageConfig);
        Destroy(gameObject);
    }
    public void SetIndicatorActive(bool isactive)
    {
        Indicator.SetActive(isactive);
    }
}
