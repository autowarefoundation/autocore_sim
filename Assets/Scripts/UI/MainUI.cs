#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion

using Assets.Scripts.Element;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class MainUI : PanelBase<MainUI>,ISimuPanel
    {
        WheelDrive wd;
        public bool isMouseOnUI;
        public Text text_Tips;

        public Button btn_carReset;
        public Button btn_carPose;
        public Button btn_addStatic;
        public Button btn_addHuman;
        public Button btn_addCarAI;
        public Button btn_addCheckPoint;
        public Button btn_cleanObstacles;
        public Button btn_saveMode;
        public Button btn_Settings;
        public Button btn_VoyageSetting;
        public Button btn_Exit;
        public Transform[] SimuTools;
        public Transform[] EditTools;
        public MonoBehaviour[] SimuScripts;
        public Image image_wheel;


        public Text text_mousePos;
        public Text text_name;
        private void Start()
        {
            Application.targetFrameRate = 60;
            wd = ObjTestCar.TestCar.WD;
            btn_carReset?.onClick.AddListener(() =>
            {
                RResetCar();
                OverLookCamera.Instance?.OLCameraReset();
            });
            btn_carPose?.onClick.AddListener(() => { ElementsManager.Instance.SetEditMode(ElementsManager.EditMode.SetCarPose); });
            btn_addStatic?.onClick.AddListener(() => { ElementsManager.Instance.SetEditMode(ElementsManager.EditMode.SetStatic); });
            btn_addHuman?.onClick.AddListener(() => { ElementsManager.Instance.SetEditMode(ElementsManager.EditMode.SetHuman); });
            btn_addCarAI?.onClick.AddListener(() => { ElementsManager.Instance.SetEditMode(ElementsManager.EditMode.SetCarAI); });
            btn_addCheckPoint?.onClick.AddListener(() => { ElementsManager.Instance.SetEditMode(ElementsManager.EditMode.SetCheckPoint); });
            btn_Exit?.onClick.AddListener(() => { PanelExit.Instance.SetPanelActive(true); });
            btn_saveMode?.onClick.AddListener(() => { TestDataManager.Instance.WriteTestJson(); });
            btn_Settings?.onClick.AddListener(() => { PanelSettings.Instance.SetPanelActive(true); });
            btn_VoyageSetting?.onClick.AddListener(() => { PanelVoyage.Instance.SetPanelActive(true); });
            btn_cleanObstacles?.onClick.AddListener(ElementsManager.Instance.RemoveAllElements);

            if (TestConfig.isEditMode)
            {
                for (int i = 0; i < SimuTools.Length; i++)
                {
                    SimuTools[i].gameObject.SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < EditTools.Length; i++)
                {
                    EditTools[i].gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < SimuScripts.Length; i++)
            {
                SimuScripts[i].enabled = !TestConfig.isEditMode;
            }
        }

        private float timeTemp;
        private float time_tipTemp;
        private void Update()
        {
            isMouseOnUI = EventSystem.current.IsPointerOverGameObject();
            CarDisShow();
            timeTemp += Time.deltaTime;
            if (timeTemp > 1)
            {
                timeTemp = 0;
            }
            if (time_tipTemp < time_tip)
            {
                time_tipTemp += Time.deltaTime;
            }
            else
            {
                text_Tips.text = string.Empty;
            }
            image_wheel.rectTransform.rotation = Quaternion.Euler(0, 0, -wd.steer * 540);
        }

        public void RResetCar()
        {
            MapManager.Instance.ResetCar();
            TestDataManager.Instance.WriteTestData("Reset ego vehicle");
        }
        public bool isCarCamera;
        private void CarDisShow()
        {
            text_mousePos.gameObject.SetActive(!isCarCamera && !isMouseOnUI);
            Vector3 offset = OverLookCamera.Instance.MouseWorldPos - ObjTestCar.TestCar.transform.position;
            float dis2Front = Mathf.Abs(Vector3.Dot(offset, ObjTestCar.TestCar.transform.forward));
            float dis2Right = Mathf.Abs(Vector3.Dot(offset, ObjTestCar.TestCar.transform.right));
            text_mousePos.rectTransform.position = new Vector3(Input.mousePosition.x + 20, Input.mousePosition.y, 0);
            text_mousePos.text = dis2Front.ToString("0.00") + "\n" + dis2Right.ToString("0.00");
        }

        private List<ISimuPanel> panels=new List<ISimuPanel>();
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
            if(panels.Contains(simuPanel)) panels.Remove(simuPanel);
        }



        public GameObject eButtonPre;
        public Transform ElementParent;
        public GameObject AddCarAIButton(GameObject car)
        {
            GameObject button = Instantiate(eButtonPre, ElementParent);
            button.GetComponent<ElementButton>().elementObj = car;
            //button.transform.SetSiblingIndex(ElementsManager.Instance.CarList.Count);
            button.name = button.transform.GetChild(0).GetComponent<Text>().text = car.name;
            ElementsManager.Instance.AddCarElement(car.GetComponent<ElementObject>());
            return button;
        }
        public GameObject AddTestCarButton(GameObject car)
        {
            GameObject button = Instantiate(eButtonPre, ElementParent);
            button.GetComponent<ElementButton>().elementObj = car;
            //button.transform.SetSiblingIndex(ElementsManager.Instance.CarList.Count);
            button.name = button.transform.GetChild(0).GetComponent<Text>().text = car.name;
            //ElementsManager.Instance.AddCarElement(car.GetComponent<ElementObject>());
            return button;
        }
        public GameObject AddHumanButton(GameObject human)
        {
            GameObject button = Instantiate(eButtonPre, ElementParent);
            button.GetComponent<ElementButton>().elementObj = human;
            //button.transform.SetSiblingIndex(ElementsManager.Instance.CarList.Count + ElementsManager.Instance.HumanList.Count);
            button.name = button.transform.GetChild(0).GetComponent<Text>().text = human.name;
            ElementsManager.Instance.AddHumanElement(human.GetComponent<ElementObject>());
            return button;
        }
        public GameObject AddTrafficLightButton(GameObject tlObj)
        {
            GameObject button = Instantiate(eButtonPre, ElementParent);
            button.GetComponent<ElementButton>().elementObj = tlObj;
            //button.transform.SetAsLastSibling();
            button.transform.GetChild(0).GetComponent<Text>().text = tlObj.name;
            ElementsManager.Instance.AddTrafficLightElement(tlObj.GetComponent<ElementObject>());
            return button;
        }
        public GameObject AddStaticButton(GameObject staticObj)
        {
            GameObject button = Instantiate(eButtonPre, ElementParent);
            button.GetComponent<ElementButton>().elementObj = staticObj;
            //button.transform.SetAsLastSibling();
            button.name = staticObj.name = button.transform.GetChild(0).GetComponent<Text>().text = staticObj.name;
            ElementsManager.Instance.AddObstacleElement(staticObj.GetComponent<ElementObject>());
            return button;
        }
        public GameObject AddCheckPointButton(GameObject checkPointObj)
        {
            GameObject button = Instantiate(eButtonPre, ElementParent);
            button.GetComponent<ElementButton>().elementObj = checkPointObj;
            //button.transform.SetAsLastSibling();
            button.name = checkPointObj.name = button.transform.GetChild(0).GetComponent<Text>().text = checkPointObj.name;
            ElementsManager.Instance.AddCheckPointElement(checkPointObj.GetComponent<ElementObject>());
            return button;
        }

        private float time_tip = 5;
        public void SetTipText(string str)
        {
            text_Tips.text = str;
            time_tipTemp = 0;
        }
    }
}