using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class PanelVoyage : PanelBase<PanelVoyage>, ISimuPanel
    {
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
            button_Close?.onClick.AddListener(() => { SetPanelActive(false); });
        }
        public void InitPanelVoyage()
        {
            //for (int i = 0; i < panelVoyageConfigs.Count; i++)
            //{
            //    Destroy(panelVoyageConfigs[i].gameObject);
            //}
            //panelVoyageConfigs.Clear();
            //foreach (VoyageConfig config in VoyageTestManager.Instance.VoyageConfigs)
            //{
            //    CreateVoyageConfig(config);
            //}
        }
        public void SetStepIndicator(int step)
        {
        }
    }
}
