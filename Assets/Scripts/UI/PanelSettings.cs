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


using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PanelSettings : SimuPanel<PanelSettings>
    {
        string[] SimuDriveModes = new string[2]
        {
            "AutowareSimu",
            "KeyboardSimu"
        };
        public Button btn_close;
        public Dropdown dropdown_DriveMode;
        public Toggle toggle_simu_ndt;
        public Toggle toggle_FollowCarPos;
        public Toggle toggle_FollowCarRot;
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
            InitDropDownItem();
            SetSimuDriveMode(TestConfig.isEditMode?1:0);
            btn_close?.onClick.AddListener(()=> { SetPanelActive(false); });
            toggle_simu_ndt.onValueChanged.AddListener(ToggleSimuNDT);
            SetCameraFollowCarPos(toggle_FollowCarPos.isOn);
            ToggleFollowCarRot(toggle_FollowCarRot.isOn);
            toggle_FollowCarPos?.onValueChanged.AddListener(SetCameraFollowCarPos);
            toggle_FollowCarRot?.onValueChanged.AddListener(ToggleFollowCarRot);
            SetMaxSteerAngle(slider_maxSteerAngle.value);
            slider_maxSteerAngle?.onValueChanged.AddListener(SetMaxSteerAngle);
            SetFriction(slider_friction.value);
            slider_friction?.onValueChanged.AddListener(SetFriction);
            SetMaxTorque(slider_maxTorque.value);
            slider_maxTorque?.onValueChanged.AddListener(SetMaxTorque);
            SetMaxSpeed(slider_maxSpeed.value);
            slider_maxSpeed?.onValueChanged.AddListener(SetMaxSpeed);
            SetCameraFollowCarPos(toggle_FollowCarPos.isOn);
            ToggleFollowCarRot(toggle_FollowCarRot.isOn);
            SetPanelActive(false);
        }
        private void InitDropDownItem()
        {
            dropdown_DriveMode.ClearOptions();
            Dropdown.OptionData temoData;
            for (int i = 0; i < SimuDriveModes.Length; i++)
            {
                temoData = new Dropdown.OptionData();
                temoData.text = SimuDriveModes[i];
                dropdown_DriveMode.options.Add(temoData);
            }
            dropdown_DriveMode.onValueChanged.AddListener(SetSimuDriveMode);
        }
        private void SetSimuDriveMode(int value)
        {
            dropdown_DriveMode.value = value;
            ObjTestCar.TestCar.WD.IsHandDrive = (value == 1);
        }
        private void ToggleFollowCarRot(bool value) => OverLookCamera.Instance.SwitchRotCam(value);

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

        private void SetMaxSpeed(float value)
        {
            if (text_maxSpeed != null)
            {
                text_maxSpeed.text = value.ToString();
            }
            ObjTestCar.TestCar.WD.maxSpeed = value/3.6f;
        }
    }

}
