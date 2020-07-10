using Assets.Scripts.Element;
using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class PanelSettings : PanelBase<PanelSettings>, ISimuPanel
    {
        public Button btn_close;
        public Toggle toggle_isFullScreen;

        public Toggle toggle_isSimuControl;
        public Dropdown dropdown_resolution;
        public Dropdown dropdown_quality;
        public Toggle toggle_SimuMessage;
        public Toggle toggle_simu_ndt;
        public Toggle toggle_FollowCarPos;
        public Toggle toggle_FollowCarRot;
        public Text text_maxCameraRange;
        public Slider slider_maxCameraRange;
        public Text text_maxSteerAngle;
        public Slider slider_maxSteerAngle;
        public Text text_friction;
        public Slider slider_friction;
        public Text text_maxTorque;
        public Slider slider_maxTorque;
        public Text text_maxSpeed;
        public Slider slider_maxSpeed;
        public LidarOrSimNdt lidarOrSimNdt;
        void Start()
        {
            InitResolutionItem();
            btn_close?.onClick.AddListener(() => { SetPanelActive(false); });
            toggle_isFullScreen?.onValueChanged.AddListener((bool value) => { isFullScreen = value; SetScreenResolution(); });
            toggle_simu_ndt?.onValueChanged.AddListener(ToggleSimuNDT);
            toggle_SimuMessage?.onValueChanged.AddListener(ToogleSimuMessagePanel);
            toggle_FollowCarPos?.onValueChanged.AddListener(SetCameraFollowCarPos);
            toggle_FollowCarRot?.onValueChanged.AddListener(SetCameraFollowCarRot);
            toggle_isSimuControl?.onValueChanged.AddListener(SetSimuDriveMode);
            slider_maxSteerAngle?.onValueChanged.AddListener(SetMaxSteerAngle);
            slider_friction?.onValueChanged.AddListener(SetFriction);
            slider_maxTorque?.onValueChanged.AddListener(SetMaxTorque);
            slider_maxCameraRange?.onValueChanged.AddListener(SetMaxCameraRange);
            slider_maxSpeed?.onValueChanged.AddListener(SetMaxSpeed);
            dropdown_quality.onValueChanged.AddListener(SetQuality);
            SetPanelActive(false); 
            PanelInit();
        }

        private void PanelInit()
        {
            SetSimuDriveMode(toggle_isSimuControl.isOn);
            ToggleSimuNDT(toggle_simu_ndt.isOn);
            ToogleSimuMessagePanel(toggle_SimuMessage.isOn);
            SetCameraFollowCarPos(toggle_FollowCarPos.isOn);
            SetCameraFollowCarRot(toggle_FollowCarRot.isOn);
            SetMaxSteerAngle(slider_maxSteerAngle.value);
            SetFriction(slider_friction.value);
            SetMaxTorque(slider_maxTorque.value);
            SetMaxCameraRange(slider_maxCameraRange.value);
            SetMaxSpeed(slider_maxSpeed.value);
            SetCameraFollowCarPos(toggle_FollowCarPos.isOn);
            SetCameraFollowCarRot(toggle_FollowCarRot.isOn);
            SetQuality(2);
        }
        private void InitResolutionItem()
        {
            dropdown_resolution.ClearOptions();
            Dropdown.OptionData temoData;
            for (int i = 0; i < DicResolution.Count; i++)
            {
                if (DicResolution.TryGetValue(i, out Resolution resolution))
                {
                    temoData = new Dropdown.OptionData();
                    temoData.text = resolution.ToString();
                    dropdown_resolution.options.Add(temoData);
                }
                else
                {
                    Debug.Log("GetResolution fail");
                }
            }
            dropdown_resolution.onValueChanged.AddListener(SetResolution);
            dropdown_resolution.value = 2;
            SetResolution(2);
        }
        public GameObject GOMaxSpd;
        public void SetSimuDriveMode(bool value)
        {
            ObjTestCar.TestCar.WD.IsHandDrive = !value; 
            SetInteractable(!value);
        }
        public void SetInteractable(bool value)
        {
            slider_maxSpeed.interactable = value;
            slider_maxTorque.interactable = value;
            slider_maxSteerAngle.interactable = value;
            toggle_isSimuControl.isOn = !value;
        }
        public void SetCameraFollowCarRot(bool value) => OverLookCamera.Instance.SwitchRotCam(value);

        public void SetCameraFollowCarPos(bool value)
        {
            if (value) OverLookCamera.Instance.OLCameraReset();
            OverLookCamera.Instance.isFollowTargetPos = value;
            toggle_FollowCarPos.isOn = value;
        }
        private void SetMaxSteerAngle(float arg0)
        {
            if (text_maxSteerAngle != null)
            {
                text_maxSteerAngle.text = string.Format(arg0.ToString());
            }
            ObjTestCar.TestCar.WD.maxAngle = arg0;
        }
        private void ToogleSimuMessagePanel(bool arg0)
        {
            PanelSimuMessage.Instance.SetPanelActive(arg0);
        }
        private void ToggleSimuNDT(bool arg0)
        {
            if (TestConfig.isEditMode) return;
            if (lidarOrSimNdt == null)
                lidarOrSimNdt = ObjTestCar.TestCar.GetComponent<LidarOrSimNdt>();
            lidarOrSimNdt.SetSimNdt(arg0);
        }
        WheelFrictionCurve forwardWFC;
        WheelFrictionCurve sideWFC;
        private void SetFriction(float arg0)
        {
            if (text_friction != null)
            {
                text_friction.text = string.Format(arg0.ToString());
            }
            foreach (var item in ObjTestCar.TestCar.WD.m_Wheels)
            {
                forwardWFC = item.forwardFriction;
                forwardWFC.stiffness = 2 * arg0;
                sideWFC = item.sidewaysFriction;
                sideWFC.stiffness = arg0;
            }
        }
        private void SetMaxTorque(float arg0)
        {
            if (text_maxTorque != null)
            {
                text_maxTorque.text = string.Format(arg0.ToString());
            }
            ObjTestCar.TestCar.WD.maxTorque = arg0;
        }
        private void SetMaxCameraRange(float arg0)
        {
            if (text_maxCameraRange != null)
            {
                text_maxCameraRange.text = string.Format(arg0.ToString());
            }
            OverLookCamera.Instance.MaxCameraSize = arg0;
        }

        private void SetMaxSpeed(float value)
        {
            if (text_maxSpeed != null)
            {
                text_maxSpeed.text = value.ToString();
            }
            ObjTestCar.TestCar.WD.maxSpeed = value / 3.6f;
        }
        private Dictionary<int, Resolution> DicResolution = new Dictionary<int, Resolution>
        {
            {0,new Resolution(2560,1440)},
            {1,new Resolution(1920,1080)},
            {2,new Resolution(1600,900)},
            {3,new Resolution(1366,768)},
            {4,new Resolution(1280,720)},
            {5,new Resolution(1024,576)}
        };
        private struct Resolution
        {
            public int width;
            public int height;

            public Resolution(int v1, int v2)
            {
                width = v1;
                height = v2;
            }
            public override string ToString()
            {
                return width.ToString() + "*" + height.ToString();
            }
        }
        private bool isFullScreen;
        private Resolution resolution;
        private void SetResolution(int value)
        {
            if (DicResolution.TryGetValue(value, out resolution))
            {
                SetScreenResolution();
            }
            else
            {
                Debug.LogError("No such Resolution");
            }
        }
        private void SetScreenResolution()
        {
            Screen.SetResolution(resolution.width, resolution.height, isFullScreen);
        }
        private void SetQuality(int lv)
        {
            QualitySettings.SetQualityLevel(lv);
        }
    }

}
