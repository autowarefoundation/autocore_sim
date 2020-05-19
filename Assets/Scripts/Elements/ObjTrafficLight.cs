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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{

    public class TrafficLightSetting
    {
        public string Name { get; set; }
        public float SwitchTime { get; set; }
        public float WaitTime { get; set; }
        public bool IsAuto { get; set; }
        public ObjTrafficLight.LightMode lightMode { get; set; }
    }
    public class ObjTrafficLight : ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            return new ElementAttbutes
            {
                attributes = new ElementAttribute[] { ElementAttribute.TrafficLight },
                name = transform.name,
                trafficLigghtAtt = new TrafficLigghtAtt
                {
                    timeSwitch = switchTime,
                    timeWait = waitTime,
                    mode = lightMode
                }
            };
        }
        private TrafficLightSetting trafficLightSetting;
        public TrafficLightSetting GetTrafficLightSetting()
        {
            trafficLightSetting = new TrafficLightSetting
            {
                Name = name,
                SwitchTime = switchTime,
                WaitTime = waitTime,
                IsAuto = isAuto,
                lightMode = lightMode
            };
            return trafficLightSetting;
        }
        public void SetTrafficLightSetting(TrafficLightSetting setting)
        {
            trafficLightSetting = setting;
            switchTime = trafficLightSetting.SwitchTime;
            waitTime = trafficLightSetting.WaitTime;
            isAuto = trafficLightSetting.IsAuto;
            lightMode = trafficLightSetting.lightMode;
            SetLight();
        }
        public enum LightMode
        {
            None = 0,
            Green = 1,
            Yellow = 2,
            Red = 3
        }
        public float switchTime = 10;//红灯时间
        public float waitTime = 3;//黄灯时间，绿灯时间=红灯-黄灯
        public bool isAuto = true;
        public LightMode lightMode = LightMode.Green;
        public GameObject RedLight;
        public GameObject YellowLight;
        public GameObject GreenLight;
        public Transform StopLine
        {
            get
            {
                return GetComponentInChildren<StopLine>().transform;
            }
        }
        public Transform TStraightDetection
        {
            get
            {
                return GetComponentInChildren<TStraightDetection>().transform;
            }
        }
        public Transform TturnRightDetection
        {
            get
            {
                return GetComponentInChildren<TTrunRightDetection>().transform;
            }
        }
        public GameObject[] Lights;
        private void Awake()
        {
            Lights = new GameObject[3] { GreenLight, YellowLight, RedLight };
        }
        ElementsManager em;
        protected override void Start()
        {
            base.Start();
            em = ElementsManager.Instance;
            ElementsManager.Instance.TrafficLightList.Add(this);
            SetLight(lightMode);

            CanScale = false;
            CanDrag = false;
            CanDelete = false;
        }
        private void SetLight()
        {
            switch (lightMode)
            {
                case LightMode.None:
                    break;
                case LightMode.Green://当前绿灯，变黄
                    lightMode = LightMode.Yellow;
                    break;
                case LightMode.Yellow://当前黄灯，变红
                    lightMode = LightMode.Red;
                    break;
                case LightMode.Red://当前红灯，变绿
                    lightMode = LightMode.Green;
                    break;
                default:
                    break;
            }
            foreach (GameObject light in Lights)
            {
                light.SetActive(false);
            }
            if (lightMode != LightMode.None) Lights[(int)lightMode - 1].SetActive(true);
        }
        public void SetLight(LightMode mode)
        {
            tempTime = 0;
            lightMode = mode;
            foreach (GameObject light in Lights)
            {
                light.SetActive(false);
            }
            if (lightMode != LightMode.None) Lights[(int)lightMode - 1].SetActive(true);
        }
        public float tempTime = 0;
        private bool isRed;
        private bool isYellow;
        public override void Update()
        {
            base.Update();
            if (!isAuto) return;
            //#region 红绿灯时间计算
            //tempTime += Time.deltaTime;
            //if (tempTime < switchTime)
            //{
            //    if (!isYellow && lightMode == LightMode.Green && tempTime + waitTime > switchTime)
            //    {
            //        SetLight();
            //    }
            //}
            //else
            //{
            //    SetLight();
            //    tempTime = 0;
            //}
            //#endregion
        }
        public override void ElementReset()
        {
            base.ElementReset();
        }
    }

}