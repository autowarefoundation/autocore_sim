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
using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    [Serializable]
    public class SimuTestMode
    {
        public string TestModeName { get; set; }
        public DateTime LastTime { get; set; }
        public string MapName { get; set; }
        public bool isRepeat { get; set; }
        public TransformData TestCarStart { get; set; }
        public List<CheckPointSetting> CheckPointSettings { get; set; }
        public List<ObstacleSetting> ObstacleSettings { get; set; }
        public List<CarAISetting> CarAISettings { get; set; }
        public List<HumanSetting> HumanSettings { get; set; }
        public List<TrafficLightSetting> TrafficLightSettings { get; set; }
        public VoyageTestConfig VoyageTestConfig { get; set; }
    }
}
