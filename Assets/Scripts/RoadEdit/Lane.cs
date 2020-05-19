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
    public class Lane : MonoBehaviour
    {
        public List<Vector3> list_Pos;
        public Vector3 PosEnd;
        public Vector3 PosStart;
        public float lenth_Lane;
        public List<Lane> SameLanes;

        public bool isReverse;
        public BezierCurve bZ;
        public BezierPoint[] bZPoints;
        public BezierPoint BZPStart
        {
            get
            {
                if (!isReverse) return bZPoints[0];
                else return bZPoints[bZPoints.Length - 1];
            }
        }
        public BezierPoint BZPEnd
        {
            get
            {
                if (!isReverse) return bZPoints[bZPoints.Length - 1];
                else return bZPoints[0];
            }
        }

        public Vector3 HandleStart
        {
            get
            {
                if (!isReverse) return bZPoints[0].handle1;
                else return bZPoints[bZPoints.Length - 1].handle1;
            }
        }
        public Vector3 endHandle
        {
            get
            {
                if (!isReverse) return bZPoints[bZPoints.Length - 1].handle1;
                else return bZPoints[0].handle1;
            }
        }
        private List<Vector3> list_PosTemp;
        public void Init(int num)
        {
            for (int i = 0; i < num; i++)
            {
                bZ.AddPointAt(Vector3.zero);
            }
            bZPoints = bZ.GetAnchorPoints();
        }

        public void Init(Vector3 PosS,Vector3 PosE,Vector3 HandleS,Vector3 HandleE)
        {
            bZ.AddPointAt(PosS);
            bZ.AddPointAt(PosE);
            bZPoints = bZ.GetAnchorPoints();
            bZPoints[0].handle1 = HandleS;
            bZPoints[1].handle1 = HandleE;
        }
        private void GetListPos(BezierPoint p1, BezierPoint p2, float tStart, float tEnd)
        {
            float tMid = (tStart + tEnd) / 2;
            Vector3 PosStart = BezierCurve.GetPoint(p1, p2, tStart);
            Vector3 PosEnd = BezierCurve.GetPoint(p1, p2, tEnd);
            float dis = Vector3.Distance(PosStart, PosEnd);
            if (dis < 3)
            {
                list_PosTemp.Add(BezierCurve.GetPoint(p1, p2, tMid));
            }
            else
            {
                GetListPos(p1, p2, tStart, tMid);
                GetListPos(p1, p2, tMid, tEnd);
            }
        }

        int index;
        float dis;
        int countRoadPos;
        Vector3 posTemp;
        public void LaneCreat()
        {
            list_PosTemp = new List<Vector3> { };
            PosStart = bZPoints[0].position;
            PosEnd = bZPoints[bZPoints.Length - 1].position;
            list_PosTemp.Add(PosStart);
            list_PosTemp.Add(PosEnd);
            for (int i = 1; i < bZPoints.Length; i++)
            {
                GetListPos(bZPoints[i - 1], bZPoints[i], 0, 1);
            }
            lenth_Lane = 0;
            countRoadPos = list_PosTemp.Count;
            list_Pos = new List<Vector3> { };
            ChangeListElement(0);
            for (int i = 1; i < countRoadPos; i++)
            {
                dis = Mathf.Infinity;
                for (int j = 0; j < list_PosTemp.Count; j++)
                {
                    float tempdis = Vector3.Distance(posTemp, list_PosTemp[j]);
                    if (tempdis < dis)
                    {
                        dis = tempdis;
                        index = j;
                    }
                }
                ChangeListElement(index);
                lenth_Lane += dis;
            }
            if (isReverse)
            {
                list_Pos.Reverse();
                var temp = PosStart;
                PosStart = PosEnd;
                PosEnd = temp;
            }
            for (int i = bZPoints.Length; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            foreach (var item in list_Pos)
            {
                GameObject obj = new GameObject();
                obj.transform.parent = transform;
                obj.transform.position = item;
            }
        }

        private void ChangeListElement(int num)
        {
            posTemp = list_PosTemp[num];
            list_Pos.Add(list_PosTemp[num]);
            list_PosTemp.RemoveAt(num);
        }
    }
}
