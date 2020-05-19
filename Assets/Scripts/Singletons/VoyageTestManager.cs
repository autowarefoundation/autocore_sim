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


using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class VoyageMode
    {
        public WheelDrive TestCar;
        public Transform Target;
        protected float angle2TestCar;
        protected float dis2TestCar;
        public float timeTemp;
        protected Vector3 offsetCar2Target;
        public virtual void Update()
        {
            offsetCar2Target = VoyageTestManager.Instance.target.position - TestCar.transform.position;
            angle2TestCar = Vector3.Angle(TestCar.transform.forward, offsetCar2Target);
            dis2TestCar = offsetCar2Target.magnitude;
            if (VoyageTestManager.Instance.IndexTest < 0) return;
            if (timeTemp > VoyageTestManager.Instance.VoyageConfigs[VoyageTestManager.Instance.IndexTest].duration)
            {
                timeTemp = 0;
                VoyageTestManager.Instance.NextStep();
            }

        }

    }
    public class VoyageNull : VoyageMode
    {
        public override void Update()
        {
            return;
        }
    }
    [Serializable]
    public class CarFollowing1 : VoyageMode
    {
        public override void Update()
        {
            base.Update();
            if (VoyageTestManager.Instance.IndexTest <0)
            {
                return;
            }
            if (TestCar.speed <= VoyageTestManager.Instance.VoyageConfigs[VoyageTestManager.Instance.IndexTest].testCarMaxSpeed
                && TestCar.speed >= VoyageTestManager.Instance.VoyageConfigs[VoyageTestManager.Instance.IndexTest].testCarMinSpeed
                && dis2TestCar <= VoyageTestManager.Instance.VoyageConfigs[VoyageTestManager.Instance.IndexTest].maxDis2Target
                && dis2TestCar >= VoyageTestManager.Instance.VoyageConfigs[VoyageTestManager.Instance.IndexTest].minDis2Target
                && angle2TestCar < 30)
            {
                timeTemp += Time.deltaTime;
            }
            else
            {
                timeTemp = 0;
            }
        }
    }


    public class VoyageTestManager : SingletonWithMono<VoyageTestManager>
    {
        public enum Mode { VoyageNull=0, CarFollowing=1 }
        public Mode mode = Mode.VoyageNull;
        [Space(3)]
        public VoyageNull voyageNull = new VoyageNull();
        public CarFollowing1 carFollowing1 = new CarFollowing1();

        public Transform target;
        private String targetName
        {
            get
            {
                if (target != null)
                    return target.name;
                else
                    return string.Empty;
            }
        }
        public VoyageMode[] voyageModes = new VoyageMode[0];

        public int IndexTest = -1;
        [SerializeField]
        private List<VoyageConfig> voyageConfigs;
        public List<VoyageConfig> VoyageConfigs
        {
            get
            {
                if (voyageConfigs == null)
                    voyageConfigs = new List<VoyageConfig>();
                return voyageConfigs;
            }
            set
            {
                voyageConfigs = value;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            voyageModes = new VoyageMode[]
                {
                voyageNull,
                carFollowing1
                };
        }
        void Start()
        {
            //VoyageTestConfig VoyageConfigs = new VoyageTestConfig
            //{
            //    voyageMode = 1,
            //    targetName = "Ai Vehicle1",
            //    configs = new List<VoyageConfig>
            //        {
            //            new VoyageConfig(0,30,5,15,10,5)
            //        }
            //};
            //SetVoyageTestConfig(VoyageConfigs);
            foreach (VoyageMode vmode in voyageModes)
            {
                vmode.TestCar = ObjTestCar.TestCar.WD;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(target!=null&& VoyageConfigs.Count > 0) voyageModes[(int)mode].Update();
        }
        public void SetVoyageTestConfig(VoyageTestConfig config)
        {
            mode = (Mode)config.voyageMode;
            VoyageConfigs = config.configs;
            var obj = GameObject.Find( config.targetName);
            if (obj != null) target = obj.transform;
            PanelVoyage.Instance.InitPanelVoyage();
            if (config.configs.Count != 0)
            {
                TestInit();
            }
        }
        public void AddTestStep(float minSpd, float maxSpd, float minDis, float maxDis, float carAISpd, float dutationtime)
        {
            if (VoyageConfigs == null) VoyageConfigs = new List<VoyageConfig>();
            VoyageConfigs.Add(new VoyageConfig(minSpd, maxSpd, minDis, maxDis, carAISpd, dutationtime));
        }
        public VoyageConfig GetLastTestConfig()
        {
            if (VoyageConfigs == null || VoyageConfigs.Count == 0) return new VoyageConfig();
            else return VoyageConfigs[VoyageConfigs.Count - 1];
        }
        public VoyageTestConfig GetVoyageTestConfig()
        {
            return new VoyageTestConfig
            {
                voyageMode = (int)mode,
                targetName = targetName,
                configs = VoyageConfigs
            };
        }
        public virtual void TestInit()
        {
            IndexTest = 0;
            PanelVoyage.Instance.SetStepIndicator(IndexTest);
            UpdateTargetSpeed();
        }
        public void TestReset()
        {
            SimuUI.Instance.SetTipText("Test Restart");
            MapManager.Instance.ResetMapElements();
            TestInit();
        }
        public void NextStep()
        {
            SimuUI.Instance.SetTipText("Next Step");
            IndexTest += 1;
            if (IndexTest >= VoyageConfigs.Count)
            {
                TestComplete();
                return;
            }
            PanelVoyage.Instance.SetStepIndicator(IndexTest);
            UpdateTargetSpeed();
        }

        public void UpdateTargetSpeed()
        {
            var obj = target.GetComponent<ElementObject>();
            if (obj!= null){
                obj.speedObjTarget= VoyageConfigs[IndexTest].targetSpeed;
            }
        }

        private void TestComplete()
        {
            TestReset();
            Debug.Log("finish");
        }
    }

}