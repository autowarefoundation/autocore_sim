#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class MainUI : PanelBase<MainUI>, ISimuPanel
    {
        public bool isMouseOnUI;
        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private float timeTemp;
        private float time_tipTemp;
        private void Update()
        {
            isMouseOnUI = EventSystem.current.IsPointerOverGameObject();
        }

        public void RResetCar()
        {
        }
        public bool isCarCameraMain;

        private List<ISimuPanel> panels = new List<ISimuPanel>();
        public void CloseLastPanel()
        {
            if (panels.Count > 0)
            {
                ISimuPanel simuPanel = panels[panels.Count - 1];
                simuPanel.SetPanelActive(false);
                panels.Remove(simuPanel);
            }
        }
        public void AddPanel(ISimuPanel simuPanel)
        {
            panels.Add(simuPanel);
        }
        public void RemovePanel(ISimuPanel simuPanel)
        {
            if (panels.Contains(simuPanel)) panels.Remove(simuPanel);
        }
        [MenuItem("Asset/CreateSimuUI")]
        public static void SetObj()
        {
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>("Packages/com.autocore.simuui/Prefabs/SimuUI.prefab");
            if (go == null)
            {
                Debug.Log("null1");
                go = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/RotateMe/Prefabs/Cube.prefab");
            }
            if (go == null)
            {
                Debug.Log("null");
            }
            else Debug.Log(go.name);
            Instantiate(go);
        }
    }
}