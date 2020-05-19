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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelVoyage : SimuPanel<PanelVoyage>
{
    public List<PanelVoyageConfig> panelVoyageConfigs;
    public GameObject PanelVoyageConfig;
    public Transform PanelConfigParent;
    public Transform AddTrans;
    public Button button_Close;
    public bool IsSetTarget
    {
        get;
        private set;
    }
    // Start is called before the first frame update
    void Start()
    {
        IsSetTarget = false;
        SetPanelActive(false);
        button_Close?.onClick.AddListener(()=> { SetPanelActive(false); }) ;
    }
    public void InitPanelVoyage()
    {
        for (int i = 0; i < panelVoyageConfigs.Count; i++)
        {
            Destroy(panelVoyageConfigs[i].gameObject);
        }
        panelVoyageConfigs.Clear();
        foreach (VoyageConfig config in VoyageTestManager.Instance.VoyageConfigs)
        {
            CreateVoyageConfig(config);
        }
    }

    public void CreateVoyageConfig(VoyageConfig voyageConfig)
    {
        PanelVoyageConfig panelVoyageConfig = Instantiate(PanelVoyageConfig, PanelConfigParent).GetComponent<PanelVoyageConfig>();
        panelVoyageConfig.InitPanel(voyageConfig);
        panelVoyageConfigs.Add(panelVoyageConfig);
        panelVoyageConfig.transform.SetAsLastSibling();
        AddTrans.SetAsLastSibling();
    }
    public void SetStepIndicator(int step)
    {
        foreach (PanelVoyageConfig panel in panelVoyageConfigs)
        {
            panel.SetIndicatorActive(false);
        }
        panelVoyageConfigs[step].SetIndicatorActive(true);
    }
}
