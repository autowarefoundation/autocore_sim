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
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TrafficLight : MonoBehaviour
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
        public Image image;
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
            if (RedLight == null) RedLight = transform.GetChild(0).gameObject;
            if (YellowLight == null) YellowLight = transform.GetChild(1).gameObject;
            if (GreenLight == null) GreenLight = transform.GetChild(2).gameObject;
            Lights = new GameObject[3] { GreenLight, YellowLight, RedLight };
        }
        ElementsManager em;
        void Start()
        {
            em = ElementsManager.Instance;
            //SetLight(lightMode);
        }
        private void SetLight()
        {
            //switch (lightMode)
            //{
            //    case LightMode.None:
            //        break;
            //    case LightMode.Green://当前绿灯，变黄
            //        lightMode = LightMode.Yellow;
            //        break;
            //    case LightMode.Yellow://当前黄灯，变红
            //        lightMode = LightMode.Red;
            //        break;
            //    case LightMode.Red://当前红灯，变绿
            //        lightMode = LightMode.Green;
            //        break;
            //    default:
            //        break;
            //}
            //foreach (GameObject light in Lights)
            //{
            //    light.SetActive(false);
            //}
            //if (lightMode != LightMode.None) Lights[(int)lightMode - 1].SetActive(true);
        }
        public void SetLight(LightMode mode)
        {
            lightMode = mode;
            foreach (GameObject light in Lights)
            {
                light.SetActive(false);
            }
            if (lightMode != LightMode.None) Lights[(int)lightMode - 1].SetActive(true);
            if (image == null)
            {
                Debug.Log(transform.name);
                return;
            }
            switch (lightMode)
            {
                case LightMode.None:
                    image.color = Color.white;
                    break;
                case LightMode.Green:
                    image.color = Color.green;
                    break;
                case LightMode.Yellow:
                    image.color = Color.yellow;
                    break;
                case LightMode.Red:
                    image.color = Color.red;
                    break;
                default:
                    break;
            }
        }
        public void SetLightImage(Image image)
        {
            this.image = image;
        }
    }

}