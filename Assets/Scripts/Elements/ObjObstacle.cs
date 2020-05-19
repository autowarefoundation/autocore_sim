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



using UnityEngine;

namespace Assets.Scripts
{
    public class ObstacleSetting
    {
        public string Name { get; set; }
        public string Pos { get; set; }
        public string Scale { get; set; }
        public string Rot { get; set; }
    }
    public class ObjObstacle : ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            return new ElementAttbutes
            {
                attributes = new ElementAttribute[] { ElementAttribute.Position, ElementAttribute.Rotation, ElementAttribute.Scale },
                name = transform.name,
                pos = transform.position,
                rot = transform.rotation.eulerAngles.y,
                sca = transform.localScale.y
            };
        }
        public ObstacleSetting obstacleSetting;
        public ObstacleSetting GetObstacleSetting()
        {
            obstacleSetting = new ObstacleSetting
            {
                Name = name,
                Pos = transform.position.ToString(),
                Scale = transform.localScale.ToString(),
                Rot = transform.rotation.eulerAngles.ToString()
            };
            return obstacleSetting;
        }
        public void SetObstacleSetting()
        {
            name = obstacleSetting.Name;
            transform.position = TestConfig.ParseV3(obstacleSetting.Pos);
            transform.localScale = TestConfig.ParseV3(obstacleSetting.Scale);
            transform.rotation = Quaternion.Euler(TestConfig.ParseV3(obstacleSetting.Rot));
        }
        protected override void Start()
        {
            base.Start();
            v3Scale = new Vector3(1, 1, 1);
            CanScale = true;
            CanDrag = true;
            CanDelete = true;
        }
        public override void Update()
        {
            offsetPos = new Vector3(0, 0.5f * v3Scale.y, 0);
            base.Update();
        }
        public override void ElementReset()
        {
            base.ElementReset();
            transform.position = TestDataManager.ParseV3(obstacleSetting.Pos);
            transform.rotation = Quaternion.Euler( TestDataManager.ParseV3(obstacleSetting.Rot));
            transform.localScale = TestDataManager.ParseV3(obstacleSetting.Scale);
        }
    }
}
