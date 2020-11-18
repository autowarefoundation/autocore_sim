#region License
/*
 * Copyright 2020 Autoware Foundation.
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
 *
 * Authors: AutoCore Members
 *
 */
#endregion


using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts
{
    public class TrafficLight : MonoBehaviour, ITrafficLight
    {
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

        //Transform StopLine { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public GameObject[] Lights;
        private void Awake()
        {
            if (RedLight == null) RedLight = transform.GetChild(0).gameObject;
            if (YellowLight == null) YellowLight = transform.GetChild(1).gameObject;
            if (GreenLight == null) GreenLight = transform.GetChild(2).gameObject;
            Lights = new GameObject[3] { GreenLight, YellowLight, RedLight };
        }
        void Start()
        {
            SetLight(lightMode);
        }
        public void SetLight(LightMode mode)
        {
            lightMode = mode;
            foreach (GameObject light in Lights)
            {
                light.SetActive(false);
            }
            if (lightMode != LightMode.None) Lights[(int)lightMode - 1].SetActive(true);
        }
        public void SetLight(int num)
        {
            lightMode = (LightMode)num;
            foreach (GameObject light in Lights)
            {
                light.SetActive(false);
            }
            if (lightMode != LightMode.None) Lights[(int)lightMode - 1].SetActive(true);
        }
        bool ITrafficLight.CanPass => lightMode == LightMode.Green;
    }

}