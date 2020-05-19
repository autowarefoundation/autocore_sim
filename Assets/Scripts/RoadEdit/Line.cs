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
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Edit
{
    public class Line: MonoBehaviour
    {
        public List<Vector3> list_Pos;
        public Vector3 PosEnd;
        public Vector3 PosStart;
        public float lenth_Lane;
        public BezierCurve bZ;
        public BezierPoint[] bZPoints;
        public GameObject LineObj;
        private List<LineObj> list_LineObjs; //line表
        public bool isRoadEdge=false;
        public void Init(int num)
        {
            for (int i = 0; i < num; i++)
            {
                bZ.AddPointAt(Vector3.zero);
            }
            bZPoints = bZ.GetAnchorPoints();
        }

        public void UpdateLines()
        {
            if (LineObj != null)
            {
                if (list_LineObjs == null) list_LineObjs = new List<LineObj>();
                list_LineObjs.Clear();
                for (int i = 1; i < bZPoints.Length; i++)
                {
                    UpdateLineObjs(bZPoints[i - 1], bZPoints[i], 0, 1);
                }
                for (int i = bZPoints.Length; i < transform.childCount; i++) DestroyImmediate(transform.GetChild(i).gameObject);
                foreach (LineObj line in list_LineObjs)
                {
                    GameObject tempLine = Instantiate(LineObj, line.position, line.quaternion, transform);
                    tempLine.transform.localScale = line.scale;
                    if (isRoadEdge) tempLine.tag = "RoadEdge";
                }
            }
        }
        /// <summary>
        /// 递归更新Line
        /// </summary>
        /// <param name="tStart">t开始</param>
        /// <param name="tEnd">t结束</param>
        private void UpdateLineObjs(BezierPoint p1,BezierPoint p2, float tStart, float tEnd)
        {
            float tMid = (tStart + tEnd) / 2;
            Vector3 PosStart = BezierCurve.GetPoint(p1, p2, tStart);
            Vector3 PosEnd = BezierCurve.GetPoint(p1, p2, tEnd);
            Vector3 midPos = BezierCurve.GetPoint(p1, p2, tMid);
            Vector3 linePos = PosStart / 2 + PosEnd / 2;
            if (Vector3.Distance(linePos, midPos) < 0.15f)
            {
                list_LineObjs.Add(new LineObj(PosStart, PosEnd));
            }
            else
            {
                UpdateLineObjs(p1, p2, tStart, tMid);
                UpdateLineObjs(p1, p2, tMid, tEnd);
            }
        }
    }
}
