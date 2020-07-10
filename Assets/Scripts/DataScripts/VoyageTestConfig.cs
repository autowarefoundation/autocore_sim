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


using System.Collections.Generic;

namespace Assets.Scripts
{
    public class VoyageConfig
    {
        public float testCarMinSpeed { get; set; }
        public float testCarMaxSpeed { get; set; }
        public float minDis2Target { get; set; }
        public float maxDis2Target { get; set; }
        public float targetSpeed { get; set; }
        public float duration { get; set; }
        public VoyageConfig() { }
        public VoyageConfig(float minSpd, float maxSpd, float minDis, float maxDis, float carAISpd, float dutationtime)
        {
            testCarMinSpeed = minSpd;
            testCarMaxSpeed = maxSpd;
            minDis2Target = minDis;
            maxDis2Target = maxDis;
            targetSpeed = carAISpd;
            duration = dutationtime;
        }
    }
    public class VoyageTestConfig 
    {
        public int voyageMode { get; set; }
        public string targetName { get; set; }
        public List<VoyageConfig> configs { get; set; }
        public VoyageTestConfig() { }
    }
}
