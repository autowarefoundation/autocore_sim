#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.SimuUI
{
    public class PanelExit : PanelBase<PanelExit>, ISimuPanel
    {
        public Button btn_ExitSimu;
        public Button btn_Cancle;
        // Start is called before the first frame update
        void Start()
        {
            SetPanelActive(false);
            btn_Cancle.onClick.AddListener(() =>
            {
                SetPanelActive(false);
            });
            btn_ExitSimu.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(0);
            });
        }
    }
}
