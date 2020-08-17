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



using Assets.Scripts.Element;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public class TestConfig
    {
        public static List<SimuTestMode> TestModes;
        private static SimuTestMode testMode;
        public static SimuTestMode TestMode
        {
            get 
            {
                if (TestModes == null) LoadAllData();
                if (testMode == null)
                    return TestModes[0];
                return testMode; 
            }
            set { testMode = value; }
        }
        public enum TestMap
        {
            City
        }
        public static TestMap testMap;
        public static bool isEditMode = false;
        private static readonly string configPath = Path.Combine(Application.streamingAssetsPath, "TestConfigs");
        public static void LoadAllData()
        {
            TestModes = new List<SimuTestMode>();
            foreach (string file in Directory.GetFiles(configPath,"*.json"))
            {
                var mode = JsonConvert.DeserializeObject<SimuTestMode>(File.ReadAllText(file));
                if (mode != null)
                {
                    TestModes.Add(mode);
                }
            } 
        }
        public static void DeleteData(SimuTestMode mode)
        {
            string path = Path.Combine(configPath, mode.TestModeName + ".json");
            if (File.Exists(path))
            {
                File.Delete(path);
                TestModes.Remove(mode);
            }
            else
            {
                Debug.Log("Delete fail");
            }
        }
        public static Vector3 ParseV3(string str)
        {
            str = str.Replace("(", "").Replace(")", "");
            string[] value = str.Split(',');
            return new Vector3(float.Parse(value[0]), float.Parse(value[1]), float.Parse(value[2]));
        }
        public static Vector3 ParseV3(Vec3 v3)
        {
            return new Vector3(v3.X,v3.Y,v3.Z);
        }
    }
}
