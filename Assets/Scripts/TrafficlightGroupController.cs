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
    public class TrafficlightGroupController : MonoBehaviour
    {

        public enum TrafficMode
        {
            Wait = 0,
            Apass = 1,
            Bpass = 2
        }
        public TrafficMode trafficMode;
        public ObjTrafficLight[] trafficLightGroupA;
        public ObjTrafficLight[] trafficLightGroupB;
        float switchTime = 10;
        public float waitTime = 3;//黄灯时间
        public float tempTime;
        private bool isApass;
        private void Start()
        {
            tempTime = 0;
            switch (trafficLightGroupA[0].lightMode)
            {
                case ObjTrafficLight.LightMode.None:
                    break;
                case ObjTrafficLight.LightMode.Green:
                    switchTime = trafficLightGroupA[0].switchTime;
                    isApass = true;
                    trafficMode = TrafficMode.Apass;
                    break;
                case ObjTrafficLight.LightMode.Yellow:
                    switchTime = trafficLightGroupA[0].waitTime;
                    trafficMode = TrafficMode.Wait;
                    break;
                case ObjTrafficLight.LightMode.Red:
                    switchTime = trafficLightGroupA[0].switchTime;
                    isApass = false;
                    trafficMode = TrafficMode.Bpass;
                    break;
                default:
                    break;
            }
            SetLights();
        }

        private void Update()
        {
            #region 红绿灯时间计算
            tempTime += Time.deltaTime;
            if (tempTime > switchTime)
            {
                switch (trafficMode)
                {
                    case TrafficMode.Wait:
                        switchTime = trafficLightGroupA[0].switchTime;
                        if (isApass) trafficMode = TrafficMode.Bpass;
                        else trafficMode = TrafficMode.Apass;
                        isApass = !isApass;
                        break;
                    case TrafficMode.Apass:
                        switchTime = waitTime;
                        trafficMode = TrafficMode.Wait;
                        break;
                    case TrafficMode.Bpass:
                        switchTime = waitTime;
                        trafficMode = TrafficMode.Wait;
                        break;
                    default:
                        break;
                }
                SetLights();
                tempTime = 0;
            }
            #endregion
        }

        public void SetLights()
        {
            switch (trafficMode)
            {
                case TrafficMode.Wait:
                    foreach (ObjTrafficLight light in trafficLightGroupA)
                    {
                        light.SetLight(ObjTrafficLight.LightMode.Yellow);
                    }
                    foreach (ObjTrafficLight light in trafficLightGroupB)
                    {
                        light.SetLight(ObjTrafficLight.LightMode.Yellow);
                    }
                    break;
                case TrafficMode.Apass:
                    foreach (ObjTrafficLight light in trafficLightGroupA)
                    {
                        light.SetLight(ObjTrafficLight.LightMode.Green);
                    }
                    foreach (ObjTrafficLight light in trafficLightGroupB)
                    {
                        light.SetLight(ObjTrafficLight.LightMode.Red);
                    }
                    break;
                case TrafficMode.Bpass:
                    foreach (ObjTrafficLight light in trafficLightGroupA)
                    {
                        light.SetLight(ObjTrafficLight.LightMode.Red);
                    }
                    foreach (ObjTrafficLight light in trafficLightGroupB)
                    {
                        light.SetLight(ObjTrafficLight.LightMode.Green);
                    }
                    break;
                default:
                    break;
            }

        }
    }
}
