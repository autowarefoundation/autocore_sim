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



using Assets.Scripts.SimuUI;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Element
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
                attributes = new bool[8] { true, true, true, true, false, false, false, true },
                name = transform.name,
                pos = transform.position,
                rot = transform.rotation.eulerAngles.y,
                sca = transform.localScale.y,
                canDelete = CanDelete
            };
        }
        public override void SetObjAttbutes(ElementAttbutes attbutes)
        {
            if (ElementsManager.Instance.SelectedElement != this) return;
            base.SetObjAttbutes(attbutes);
            transform.position = attbutes.pos;
            transform.rotation = Quaternion.Euler(new Vector3(0, attbutes.rot, 0));
            transform.localScale = attbutes.sca * new Vector3(1, 1, 1);
        }
        private ObstacleSetting obstacleSetting;
        public ObstacleSetting ObstacleSetting
        {
            get
            {
                if (obstacleSetting == null)
                    obstacleSetting = new ObstacleSetting
                    {
                        Name = name,
                        Pos = transform.position.ToString(),
                        Scale = transform.localScale.ToString(),
                        Rot = transform.rotation.eulerAngles.ToString()
                    };
                return obstacleSetting;
            }
            set
            {
                obstacleSetting = value;
            }
        }
        protected override void Start()
        {
            nameLogic = "ObstacleLogic";
            base.Start();
            v3Scale = new Vector3(1, 1, 1);
            CanScale = true;
            CanDrag = true;
            CanDelete = true;
        }
        protected override void Update()
        {
            offsetPos = new Vector3(0, 0.5f * v3Scale.y, 0);
            base.Update(); 
        }
        public override void ElementReset()
        {
            base.ElementReset();
            name = ObstacleSetting.Name;
            transform.position = TestDataManager.ParseV3(ObstacleSetting.Pos);
            transform.rotation = Quaternion.Euler( TestDataManager.ParseV3(ObstacleSetting.Rot));
            transform.localScale = TestDataManager.ParseV3(ObstacleSetting.Scale);
        }
    }
}
