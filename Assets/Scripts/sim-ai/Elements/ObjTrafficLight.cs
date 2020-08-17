using UnityEngine;

using Assets.Scripts.SimuUI;
using UnityEditor;

namespace Assets.Scripts.Element
{
    public class TrafficLightSetting
    {
        public string Name { get; set; }
        public float SwitchTime { get; set; }
        public float WaitTime { get; set; }
        public bool IsAuto { get; set; }
        public ObjTrafficLight.TrafficMode lightMode { get; set; }
    }
    public class ObjTrafficLight : ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            return new ElementAttbutes
            {
                attributes = new bool[8] { true, false, false, false, false, true, false, false },
                name = transform.name,
                trafficLigghtAtt = new TrafficLigghtAtt
                {
                    timeSwitch = switchTime,
                    timeWait = waitTime,
                    mode = (int)trafficMode
                },
                canDelete = CanDelete
            };
        }
        public override void SetObjAttbutes(ElementAttbutes attbutes)
        {
            if (ElementsManager.Instance.SelectedElement != this) return;
            base.SetObjAttbutes(attbutes);
            switchTime = attbutes.trafficLigghtAtt.timeSwitch;
            waitTime = attbutes.trafficLigghtAtt.timeWait;
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
                    lightMode = trafficMode
                };
            }
            set
            {
                switchTime = value.SwitchTime;
                waitTime = value.WaitTime;
                trafficMode = value.lightMode;
                SetLights();
            }
        }
        public enum TrafficMode
        {
            Wait = 0,
            Apass = 1,
            Bpass = 2
        }
        public TrafficMode trafficMode = TrafficMode.Apass;
        public TrafficLight[] trafficLightGroupA;
        public TrafficLight[] trafficLightGroupB;
        public float switchTime = 10;
        public float waitTime = 3;//黄灯时间
        private float tempSwtichTime=0;
        public float tempTime = 0;
        private bool isApass;
        private LogicTrafficLight ltl;
        protected override void Start()
        {
            nameLogic = "TrafficLightLogic";
            base.Start();
            if(!ElementsManager.Instance.TrafficLightList.Contains(this))
            ElementsManager.Instance.TrafficLightList.Add(this);
            CanScale = false;
            CanDrag = false;
            CanDelete = false;
            ltl = logicObject.GetComponent<LogicTrafficLight>();
            trafficLightGroupA = transform.GetChild(0).GetComponentsInChildren<TrafficLight>();
            trafficLightGroupB = transform.GetChild(1).GetComponentsInChildren<TrafficLight>();
            SetLights();
        }

        protected override void Update()
        {
            #region 红绿灯时间计算
            tempTime += Time.deltaTime;
            ltl?.SetLogicText((int)(tempSwtichTime - tempTime) + 1);
            if (tempTime > tempSwtichTime)
            {
                SwitchLight();
            }
            #endregion
        }
        public void SwitchLight()
        {
            switch (trafficMode)
            {
                case TrafficMode.Wait:
                    tempSwtichTime = switchTime;
                    if (isApass) trafficMode = TrafficMode.Bpass;
                    else trafficMode = TrafficMode.Apass;
                    isApass = !isApass;
                    break;
                case TrafficMode.Apass:
                    tempSwtichTime = waitTime;
                    trafficMode = TrafficMode.Wait;
                    break;
                case TrafficMode.Bpass:
                    tempSwtichTime = waitTime;
                    trafficMode = TrafficMode.Wait;
                    break;
                default:
                    break;
            }
            SetLights();
            tempTime = 0;
        }
        public void SetLights()
        {
            switch (trafficMode)
            {
                case TrafficMode.Wait:
                    foreach (TrafficLight light in trafficLightGroupA)
                    {
                        light.SetLight(TrafficLight.LightMode.Yellow);
                    }
                    foreach (TrafficLight light in trafficLightGroupB)
                    {
                        light.SetLight(TrafficLight.LightMode.Yellow);
                    }
                    break;
                case TrafficMode.Apass:
                    foreach (TrafficLight light in trafficLightGroupA)
                    {
                        light.SetLight(TrafficLight.LightMode.Green);
                    }
                    foreach (TrafficLight light in trafficLightGroupB)
                    {
                        light.SetLight(TrafficLight.LightMode.Red);
                    }
                    break;
                case TrafficMode.Bpass:
                    foreach (TrafficLight light in trafficLightGroupA)
                    {
                        light.SetLight(TrafficLight.LightMode.Red);
                    }
                    foreach (TrafficLight light in trafficLightGroupB)
                    {
                        light.SetLight(TrafficLight.LightMode.Green);
                    }
                    break;
                default:
                    break;
            }
            ltl.SetLogicTrafficLight((int)trafficMode);
        }
    }
}
