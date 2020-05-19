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



using Packages.BezierCurveEditorPackage.Scripts;
using UnityEngine;

namespace Assets.Scripts.Edit
{
    public class RoadEditor : MonoBehaviour
    {
        public Transform PointS;
        public Transform PointE;
        public Transform HandleS;
        public Transform HandleE;
        public BezierCurve bZ;
        public BezierPoint sBZPoint, eBZPoint;
        private void Start()
        {
            PointS = transform.GetChild(0);
            PointE = transform.GetChild(1);
            HandleS = PointS.transform.GetChild(0);
            HandleE = PointE.transform.GetChild(0);
            if (GetComponent<BezierCurve>()) bZ = GetComponent<BezierCurve>();
            else bZ = gameObject.AddComponent<BezierCurve>();
            sBZPoint = bZ.AddPointAt(transform.position);
            sBZPoint.handle1 = Vector3.forward * 5;
            eBZPoint = bZ.AddPointAt(new Vector3(0, 0, -20) + transform.position);
            eBZPoint.handle1 = Vector3.forward * 5;
        }
        public void InitEditor()
        {
            SetEditor();
        }
        public void SetEditor()
        {
            PointS.position = sBZPoint.position;
            PointE.position = eBZPoint.position;
            HandleS.position = sBZPoint.position+sBZPoint.handle2;
            HandleE.position = eBZPoint.position + eBZPoint.handle1;
        }

        private void Update()
        {
            //if (CameraOP.isDrag)
            //{
            //    RoadManager.selectedRoad.transform.position = sBZPoint.position;
            //    RoadManager.selectedRoad.sBZPoint.position = sBZPoint.position;
            //    RoadManager.selectedRoad.eBZPoint.position = eBZPoint.position;
            //    RoadManager.selectedRoad.sBZPoint.handle1 = sBZPoint.handle1;
            //    RoadManager.selectedRoad.eBZPoint.handle1 = eBZPoint.handle1;
            //    RoadManager.selectedRoad.LinesSet();
            //}
            //else
            //{
            //    SetEditor();
            //}

            sBZPoint.position = PointS.position;
            eBZPoint.position = PointE.position;
            sBZPoint.handle2 = HandleS.position - PointS.position;
            eBZPoint.handle1 = HandleE.position - PointE.position;
        }
    }

}
