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
using UnityEngine;

namespace Assets.Scripts.Element
{
    public class CheckPointSetting
    {
        public string Name { get; set; }

        public TransformData transformData { get; set; }
    }
    public class ObjCheckPoint : ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            return new ElementAttbutes
            {
                attributes = new bool[8] { true, true, true, false, false, false,false, true },
                name = transform.name,
                pos = transform.position,
                rot=transform.rotation.eulerAngles.y,
                sca = transform.localScale.y,
                canDelete = CanDelete
            };
        }
        public CheckPointSetting chechPointSetting;
        public CheckPointSetting GetCheckPointSetting()
        {
            TransformData data = new TransformData(new Vec3(transform.position), new Vec3(transform.rotation.eulerAngles), new Vec3(transform.localScale));
            chechPointSetting = new CheckPointSetting
            {
                Name = gameObject.name,
                transformData = data
            };
            return chechPointSetting;
        }
        public override void SetObjAttbutes(ElementAttbutes attbutes)
        {
            if (ElementsManager.Instance.SelectedElement != this) return;
            base.SetObjAttbutes(attbutes);
            transform.position = attbutes.pos;
            transform.rotation = Quaternion.Euler(new Vector3(0, attbutes.rot, 0));
            transform.localScale = attbutes.sca * new Vector3(1, 1, 1);
        }
        public void SetObstacleSetting()
        {
            if (chechPointSetting != null)
            {
                transform.name = chechPointSetting.Name;
                transform.position = TestConfig.ParseV3(chechPointSetting.transformData.V3Pos);
                transform.rotation = Quaternion.Euler(TestConfig.ParseV3(chechPointSetting.transformData.V3Rot));
                transform.localScale = TestConfig.ParseV3(chechPointSetting.transformData.V3Sca);
            }
        }
        protected override void Start()
        {
            nameLogic = "CheckPointLogic";
            base.Start();
            v3Scale = new Vector3(3, 1, 5);
            CanScale = true;
            CanDrag = true;
            CanDelete = true;
        }
        protected override void Update()
        {
            offsetPos = new Vector3(0, -0.5f*v3Scale.y, 0);
            base.Update();
        }
        public override void ElementReset()
        {
            base.ElementReset(); 
            SetObstacleSetting();
        }
    }
}
