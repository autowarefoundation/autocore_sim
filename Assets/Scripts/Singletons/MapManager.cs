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


using Assets.Scripts.Edit;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class MapManager : SingletonWithMono<MapManager>
    {
        private Roads roads;
        public List<Lane> Lanes;
        public bool isRepeat=false;
        protected override void Awake()
        {
            base.Awake();
        }
        void Start()
        {
            Mapinit();
            TestDataManager.Instance.TDMInit();
        }
        public void Mapinit()
        {
            ElementsManager.Instance.SwitchCheckPoint();
            roads = GetComponentInChildren<Roads>();
            Lanes = roads.list_Lane;
            ElementsManager.Instance.RemoveAllElements();
            SetMapElements();
        }
        public void SetMapElements()
        {
            if(TestConfig.TestMode.TestCarStart!=null) ResetCar();
            isRepeat = TestConfig.TestMode.isRepeat;
            if (TestConfig.TestMode.CheckPointSettings!=null&& TestConfig.TestMode.CheckPointSettings.Count>0)
            {
                foreach (CheckPointSetting setting in TestConfig.TestMode.CheckPointSettings)
                {
                    ElementsManager.Instance.AddCheckPoint(TestConfig.ParseV3(setting.transformData.V3Pos), TestConfig.ParseV3(setting.transformData.V3Rot), TestConfig.ParseV3(setting.transformData.V3Sca), setting.Name).chechPointSetting=setting;
                   
                }
            }
            if (TestConfig.TestMode.ObstacleSettings != null)
            {
                foreach (ObstacleSetting setting in TestConfig.TestMode.ObstacleSettings)
                {
                    ElementsManager.Instance.AddObstacle(TestDataManager.ParseV3(setting.Pos), TestDataManager.ParseV3(setting.Rot), TestDataManager.ParseV3(setting.Scale), setting.Name);
                }
            }
            if (TestConfig.TestMode.CarAISettings != null)
            {
                foreach (CarAISetting setting in TestConfig.TestMode.CarAISettings)
                {
                    ElementsManager.Instance.AddCarAI(TestDataManager.ParseV3(setting.PosInit), setting.Name).SetCarAISetting(setting);
                }
            }
            if (TestConfig.TestMode.HumanSettings != null)
            {
                foreach (HumanSetting setting in TestConfig.TestMode.HumanSettings)
                {
                    ElementsManager.Instance.AddPedestrian(TestDataManager.ParseV3(setting.PosArray[0]), setting.Name).SetHumansetting(setting);
                }
            }
            if (TestConfig.TestMode.TrafficLightSettings != null)
            {
                foreach (TrafficLightSetting setting in TestConfig.TestMode.TrafficLightSettings)
                {
                    foreach (ObjTrafficLight item in ElementsManager.Instance.TrafficLightList)
                    {
                        if (item.name == setting.Name)
                        {
                            item.SetTrafficLightSetting(setting);
                        }
                    }
                }
            }
            if (TestConfig.TestMode.VoyageTestConfig != null)
            {
                VoyageTestManager.Instance.SetVoyageTestConfig(TestConfig.TestMode.VoyageTestConfig);
            }
        }
        public void ResetMapElements()
        {
            if (TestConfig.TestMode.TestCarStart != null) ResetCar();
            foreach (ElementObject obj in ElementsManager.Instance.CarList)
            {
                var objCarAI = obj.GetComponent<ObjAICar>();
                if(objCarAI!=null) objCarAI.ElementReset();
            }
            foreach (ElementObject obj in ElementsManager.Instance.HumanList)
            {
                var objHuman = obj.GetComponent<ObjHuman>();
                if(objHuman!=null) objHuman.ElementReset();
            }
            foreach (ElementObject obj in ElementsManager.Instance.ObstacleList)
            {
                var objObstacle = obj.GetComponent<ObjObstacle>();
                if (objObstacle != null) objObstacle.ElementReset();
            }
            foreach (ElementObject obj in ElementsManager.Instance.CheckPointList)
            {
                var objCheckPoint = obj.GetComponent<ObjCheckPoint>();
                if (objCheckPoint != null) objCheckPoint.ElementReset();
            }
        }

        public void ResetCar()
        {
            ObjTestCar.TestCar.WD.Reset(TestConfig.ParseV3(TestConfig.TestMode.TestCarStart.V3Pos), Quaternion.Euler(TestConfig.ParseV3(TestConfig.TestMode.TestCarStart.V3Rot)));
            ObjTestCar.TestCar.WD.GetComponent<SpeedController>().LinearVelocity = 0;
            ObjTestCar.TestCar.WD.GetComponent<SpeedController>().SteeringAngle = 0;
        }

        public Lane SearchNearestPos2Lane(out int index, Vector3 positon)
        {
            if (Lanes == null) Debug.Log("lanes is null");
            float disMin = Mathf.Infinity;
            Lane laneTemp=Lanes[0];
            int indexTemp = 0;
            foreach (Lane lane in Lanes)
            {
                foreach (Vector3 pos in lane.list_Pos)
                {
                    float dis = Vector3.Distance(pos, positon);
                    if (dis < disMin)
                    {
                        disMin = dis;
                        indexTemp = lane.list_Pos.IndexOf(pos);
                        if (indexTemp == lane.list_Pos.Count - 1) indexTemp--;
                        laneTemp = lane;
                    }
                }
            }
            index = indexTemp + 1;
            return laneTemp;
        }
    }
}
