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


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public class TestDataManager:Singleton<TestDataManager>
    {
        public override void Init()
        {
            Debug.Log("init TestDataManager");
        }
        public string testModeName;

        public void TDMInit()
        {
            testModeName = TestConfig.TestMode.TestModeName;
            dataFilePath = Application.streamingAssetsPath + @"\TestData\" + DateTime.Now.ToString("yyyyYMMMddDHH_ mm_ss") + ".txt";
            WriteTestData(DateTime.Now.ToString("yyyyYMMMddDHH_ mm_ss") +"TestStart");
        }
        public void AddTestMode(string modename,string mapname)
        {
            testModeName = modename;
            TestConfig.testMap = (TestConfig.TestMap)Enum.Parse(typeof(TestConfig.TestMap), mapname);
            WriteTestJson(true);
        }

        public void WriteTestJson(bool isNew=false)
        {
            SimuTestMode td = new SimuTestMode();
            td.TestModeName = testModeName;
            td.MapName = TestConfig.testMap.ToString();
            td.LastTime = DateTime.Now;
            td.VoyageTestConfig = VoyageTestManager.Instance.GetVoyageTestConfig();
            if (isNew)
            {
                td.TestCarStart = new TransformData (new Vec3( -200.0f, 0.0f, -4.5f), new Vec3(0.0f, 90.0f, 0.0f) , new Vec3(1f,1f,1f));
                td.CheckPointSettings = new List<CheckPointSetting>
                {
                    new CheckPointSetting
                    {
                        Name="CheckPoint0",
                        transformData=new TransformData(new Vec3(50.0f, -0.5f, 0.0f), new Vec3(0.0f, 0.0f, 0.0f), new Vec3(1f, 1f, 1f))
                    }
                };
            }
            else
            {
                td.TestCarStart = new TransformData(ObjTestCar.TestCar.transform);
                foreach (ElementObject item in ElementsManager.Instance.CheckPointList)
                {
                    var objCheckPoint= item.GetComponent<ObjCheckPoint>();
                    if (objCheckPoint == null) continue;
                    if (td.CheckPointSettings == null) td.CheckPointSettings = new List<CheckPointSetting>();
                    td.CheckPointSettings.Add(objCheckPoint.GetCheckPointSetting());
                }
                foreach (ElementObject item in ElementsManager.Instance.ObstacleList)
                {
                    var objObstacle = item.GetComponent<ObjObstacle>();
                    if (objObstacle == null) continue;
                    if (td.ObstacleSettings == null) td.ObstacleSettings = new List<ObstacleSetting>();
                    td.ObstacleSettings.Add(objObstacle.GetObstacleSetting());
                }
                foreach (ElementObject item in ElementsManager.Instance.CarList)
                {
                    var objCar = item.GetComponent<ObjAICar>();
                    if (objCar == null) continue;
                    if (td.CarAISettings == null) td.CarAISettings = new List<CarAISetting>();
                    td.CarAISettings.Add(objCar.GetCarAISetting());
                }
                foreach (ElementObject item in ElementsManager.Instance.HumanList)
                {
                    var objhuman = item.GetComponent<ObjHuman>();
                    if (objhuman == null) continue;
                    if (td.HumanSettings == null) td.HumanSettings = new List<HumanSetting>();
                    td.HumanSettings.Add(objhuman.GetHumansetting());
                }
                foreach (ElementObject item in ElementsManager.Instance.TrafficLightList)
                {
                    var objTL = item.GetComponent<ObjTrafficLight>();
                    if (objTL == null) continue;
                    if (td.TrafficLightSettings == null) td.TrafficLightSettings = new List<TrafficLightSetting>();
                    td.TrafficLightSettings.Add(objTL.GetTrafficLightSetting());
                }
            }
            string content = JsonConvert.SerializeObject(td);
            WriteByLineCover(Application.streamingAssetsPath + @"\TestConfigs\" + td.TestModeName + ".json", content);
            if (SimuUI.Instance != null)
            {
                SimuUI.Instance.SetTipText("Mode Save OK");
            }
            TestConfig.TestMode = td;
        }
        FileStream fStream;
        StreamWriter sw;

        private string dataFilePath;
        public void WriteTestData( string content)
        {
            var dic = Path.GetDirectoryName(dataFilePath);
            if (!Directory.Exists(dic))
            {
                Directory.CreateDirectory(dic);
            }
            if (!File.Exists(dataFilePath))
            {
                File.CreateText(dataFilePath).Dispose();
            }
            WriteFileByLine(dataFilePath, DateTime.Now+" " + content);
        }

        public void WriteFileByLine(string strPath, string value)
        {
            try
            {
                sw = new StreamWriter(strPath, true);
                sw.WriteLine(value);
                sw.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        string testConfigPath;
        public void WriteByLineCover(string strPath, string content)
        {
            testConfigPath = strPath;
            fStream = File.Open(testConfigPath, FileMode.OpenOrCreate, FileAccess.Write);
            fStream.Seek(0, SeekOrigin.Begin);
            fStream.SetLength(0);
            fStream.Close();
            WriteFileByLine(testConfigPath, content);
        }

        public static Vector3 ParseV3(string str)
        {
            str = str.Replace("(", "").Replace(")", "");
            string[] value = str.Split(',');
            return new Vector3(float.Parse(value[0]), float.Parse(value[1]), float.Parse(value[2]));
        }
        private void OnDestroy()
        {
            if (sw != null) sw.Dispose();
        }
    }
}
