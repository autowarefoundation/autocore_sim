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
        public int IndexLightsGroup { get; set; }
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
                    index = indexPassGroup
                }
            };
        }
        public TrafficLightSetting TrafficLightSetting
        {
            get
            {
                return new TrafficLightSetting
                {
                    Name = name,
                    SwitchTime = switchTime,
                    WaitTime = waitTime,
                    IndexLightsGroup = indexPassGroup
                };
            }
            set
            {
                switchTime = value.SwitchTime;
                waitTime = value.WaitTime;
                indexPassGroup = value.IndexLightsGroup;
                SetLights();
            }
        }
        public int indexPassGroup=0;
        private bool isYellow = false;
        public List<TrafficLight[]> trafficLightsList=new List<TrafficLight[]>();
        public float switchTime = 10;
        public float waitTime = 3;//黄灯时间
        public float tempTime;
        private bool isApass;
        private LogicTrafficLight ltl;
        protected override void Start()
        {
            nameLogic = "TrafficLightLogic";
            base.Start();
            ElementsManager.Instance.TrafficLightList.Add(this);
            CanScale = false;
            CanDrag = false;
            CanDelete = false;
            tempTime = 0;
            ltl = logicObject.GetComponent<LogicTrafficLight>();
            for (int i = 0; i < transform.childCount; i++)
            {
                TrafficLight[] trafficLightsTemp = transform.GetChild(i).GetComponentsInChildren<TrafficLight>();
                if(trafficLightsTemp.Length>0)trafficLightsList.Add(trafficLightsTemp);
            }
            //SetLights();
        }

        protected override void Update()
        {
            #region 红绿灯时间计算
            tempTime += Time.deltaTime;
            ltl.SetLogicText((int)(switchTime - tempTime)+1);
            if (switchTime - tempTime <= waitTime&&!isYellow)
            {
                isYellow = true;
                foreach (TrafficLight light in trafficLightsList[indexPassGroup])
                {
                    light.SetLight(TrafficLight.LightMode.Yellow);
                }
            }
            if (tempTime >= switchTime)
            {
                SwitchLight();
            }
            #endregion
        }
        public void SwitchLight()
        {
            indexPassGroup++;
            if (indexPassGroup >= trafficLightsList.Count) indexPassGroup = 0;
            SetLights();
            isYellow = false;
            tempTime = 0;
        }
        bool isPass;
        public void SetLights()
        {
            for (int i = 0; i < trafficLightsList.Count; i++)
            {
                isPass = i == indexPassGroup;
                for (int j = 0; j < trafficLightsList[i].Length; j++)
                {
                    trafficLightsList[i][j].SetLight(isPass ? TrafficLight.LightMode.Green : TrafficLight.LightMode.Red);
                }
            }
        }
    }
} 
