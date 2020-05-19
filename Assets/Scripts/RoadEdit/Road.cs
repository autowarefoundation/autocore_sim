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
    //[ExecuteInEditMode]
    public class Road : MonoBehaviour
    {
        public Roads roads;
        public bool isCurvatureSame = false;
        //需要设置的
        public Roads.RoadKind roadKind; //道路类型
        public float roadLenth=20;//路长
        public float roadWidth;//路宽
        public float YellowLineSpace;
        public float laneWidth=3;//车道宽
        private List<float> OffsetLine;
        private List<float> OffsetLane;
        private BezierCurve bZ; //贝塞尔曲线
        public BezierPoint[] bZPoints;
        public Vector3[,] PosBzPoints;
        public Transform roadEdges;
        public Transform roadLines;
        public Transform RoadLanes;
        public Transform midLines;
        public int[] lineKindArr; //road的line类型
        public List<Line> LineArr; //road的line
        public List<Lane> LaneArr; //road的lane
        public Vector3 roadCenterPos;

        private Vector3[] handle2_Unit;
        private Vector3[] handle1_Unit;
        Vector3[] Pos_Intersection;
        float[] Dis_S2P;
        float[] Dis_E2P;
        public Vector3[,] ArrLinePoints;
        private Vector3[,] ArrLanePoints;

        private void Start()
        {
            RoadInit();
        }
        void Update()
        {
            if (!IsPosBZPointsSame())
            {
                SetPosBZPoints();
                foreach (var item in bZPoints)
                {
                    if (item.position.y != 0) item.position = new Vector3(item.position.x, 0, item.position.z);
                    if (item.handle1.y != 0) item.handle1 = new Vector3(item.handle1.x, 0, item.handle1.z);
                }
            } 
        }
        private int laneCount;
        private int lineCount;

        private bool IsPosBZPointsSame()
        {
            if (PosBzPoints == null)
            {
                return false;
            }
            if (PosBzPoints.GetLength(0) != bZPoints.Length)
            {
                return false;
            }
            for (int i = 0; i < bZPoints.Length; i++)
            {
                if (PosBzPoints[i, 0] != bZPoints[i].position)
                {
                    return false;
                }
                if (PosBzPoints[i, 1] != bZPoints[i].handle1)
                {
                    return false;
                }
            }
            return true;
        }
        private void SetPosBZPoints()
        {
            PosBzPoints = new Vector3[bZPoints.Length, 2];
            for (int i = 0; i < bZPoints.Length; i++)
            {
                PosBzPoints[i, 0] = bZPoints[i].position;
                PosBzPoints[i, 1] = bZPoints[i].handle1;
            }
        }

        public void RoadInit()
        {
            Debug.Log("init");
            roads = GetComponentInParent<Roads>();
            bZ = transform.GetComponent<BezierCurve>();
            bZPoints = bZ.GetAnchorPoints();
            PosBzPoints = new Vector3[bZPoints.Length, 2];
            if (!IsPosBZPointsSame()) SetPosBZPoints();
            if (roadEdges == null)
                roadEdges = transform.GetChild(3);
            if (roadLines == null)
                roadLines = transform.GetChild(2);
            if (midLines == null)
                midLines = transform.GetChild(4);
            if (RoadLanes == null)
            {
                GameObject obj = new GameObject(gameObject.name+"Lanes");
                obj.transform.SetParent(roads.Trans_RoadLanes);
                RoadLanes = obj.transform;
            }
            else
            {
                RoadLanes.gameObject.name = gameObject.name + "Lanes";
            }
            LineArr = new List<Line> { };
            LaneArr = new List<Lane> { };
            if (roads.RoadDictionry.TryGetValue(roadKind, out lineKindArr))
            {
                laneCount = lineKindArr[0];
                lineCount = lineKindArr[1];
                OffsetLine = new List<float> { };
                OffsetLane = new List<float> { };
                LineCreat(0);
                OffsetLine.Add(YellowLineSpace / 2);
                if (YellowLineSpace != 0)
                {
                    LineCreat(0);
                    OffsetLine.Add(-YellowLineSpace / 2);
                }
                for (int i = 0; i < laneCount / 2; i++)
                {
                    LineCreat(1);
                    LineCreat(1);
                    OffsetLane.Add(laneWidth * (i + 0.5f)+ YellowLineSpace / 2);
                    OffsetLane.Add(-laneWidth * (i + 0.5f)- YellowLineSpace / 2);
                }
                for (int i = 0; i < lineCount / 2; i++)
                {
                    LineCreat(2);
                    LineCreat(2);
                    OffsetLine.Add(laneWidth * (i + 1f));
                    OffsetLine.Add(-laneWidth * (i + 1f));
                }
                LineCreat(3);
                LineCreat(3);
                OffsetLine.Add(laneWidth * lineCount / 2 + 0.2f);
                OffsetLine.Add(-laneWidth * lineCount / 2 - 0.2f);
                LaneSameLanesSet();
            }
            else
            {
                Debug.LogError("no midLine");
            }
            handle1_Unit = new Vector3[bZPoints.Length];
            handle2_Unit = new Vector3[bZPoints.Length];
            Pos_Intersection = new Vector3[bZPoints.Length - 1];
            Dis_E2P = new float[bZPoints.Length - 1];
            Dis_S2P = new float[bZPoints.Length - 1];

            LanesSet();
            LinesSet();
        }
        public void DestroyAllLines()
        {
            while (roadEdges.childCount > 0)
            {
                DestroyImmediate(roadEdges.GetChild(0).gameObject);
            }
            while (roadLines.childCount > 0)
            {
                DestroyImmediate(roadLines.GetChild(0).gameObject);
            }
            while (midLines.childCount > 0)
            {
                DestroyImmediate(midLines.GetChild(0).gameObject);
            }
            while (RoadLanes.childCount > 0)
            {
                DestroyImmediate(RoadLanes.GetChild(0).gameObject);
            }
        }
        public void SetAllLines()
        {
            LanesSet();
            LinesSet();
        }

        //设置Lines
        public void LinesSet()
        {
            ArrLinePoints = new Vector3[bZPoints.Length, LineArr.Count];
            for (int i = 0; i < bZPoints.Length; i++)
            {
                handle1_Unit[i] = Quaternion.AngleAxis(90, Vector3.up) * bZPoints[i].handle1.normalized;
                handle2_Unit[i] = Quaternion.AngleAxis(90, Vector3.up) * bZPoints[i].handle2.normalized;
                if (i > 0)
                {
                    Pos_Intersection[i - 1] = GetSamePos(handle2_Unit[i - 1], bZPoints[i - 1].position, handle1_Unit[i], bZPoints[i].position);
                    //Debug.Log("v1:"+handle2_Unit[i - 1]+ "v2:" + handle2_Unit[i]+ "p1:" + bZPoints[i - 1].position + "v1:" + bZPoints[i].position+"inte"+ Pos_Intersection[i - 1]);
                    Dis_S2P[i - 1] = Vector3.Distance(bZPoints[i - 1].position, Pos_Intersection[i - 1]);
                    Dis_E2P[i - 1] = Vector3.Distance(bZPoints[i].position, Pos_Intersection[i - 1]);
                }
                for (int j = 0; j < LineArr.Count; j++)
                {
                    ArrLinePoints[i, j] = bZPoints[i].position + handle2_Unit[i] * OffsetLine[j];
                }
            }
            for (int i = 0; i < LineArr.Count; i++)
            {
                for (int j = 0; j < bZPoints.Length; j++)
                {
                    LineArr[i].bZPoints[j].position = ArrLinePoints[j, i];
                    if (j > 0)
                    {
                        if (Pos_Intersection[j - 1] == Vector3.zero)
                        {
                            continue;
                        }
                        LineArr[i].bZPoints[j].handleStyle = bZPoints[j].handleStyle;
                        float ratio_Start = Vector3.Distance(ArrLinePoints[j-1,i], Pos_Intersection[j - 1]) / Dis_S2P[j - 1];
                        float ratio_End = Vector3.Distance(ArrLinePoints[j, i], Pos_Intersection[j - 1]) / Dis_E2P[j - 1];

                        LineArr[i].bZPoints[j - 1].handle1 = ratio_Start * bZPoints[j - 1].handle1;
                        LineArr[i].bZPoints[j - 1].handle2 = ratio_Start * bZPoints[j - 1].handle2;
                        LineArr[i].bZPoints[j].handle1 = ratio_End * bZPoints[j].handle1;
                        LineArr[i].bZPoints[j].handle2 = ratio_End * bZPoints[j].handle2;
                    }
                }
            }
        }
        public void LanesSet()
        {
            ArrLanePoints = new Vector3[bZPoints.Length, LaneArr.Count];
            for (int i = 0; i < bZPoints.Length; i++)
            {
                handle1_Unit[i] = Quaternion.AngleAxis(90, Vector3.up) * bZPoints[i].handle1.normalized;
                if (i > 0)
                {
                    Pos_Intersection[i - 1] = GetSamePos(handle1_Unit[i - 1], bZPoints[i - 1].position, handle1_Unit[i], bZPoints[i].position);
                    Dis_S2P[i - 1] = Vector3.Distance(bZPoints[i - 1].position, Pos_Intersection[i - 1]);
                    Dis_E2P[i - 1] = Vector3.Distance(bZPoints[i].position, Pos_Intersection[i - 1]);
                }
                for (int j = 0; j < LaneArr.Count; j++)
                {
                    ArrLanePoints[i, j] = bZPoints[i].position + handle1_Unit[i] * OffsetLane[j];
                }
            }
            for (int i = 0; i < LaneArr.Count; i++)
            {
                for (int j = 0; j < bZPoints.Length; j++)
                {
                    LaneArr[i].bZPoints[j].position = ArrLanePoints[j, i];
                    if (j > 0)
                    {
                        if (Pos_Intersection[j - 1] == Vector3.zero)
                        {
                            continue;
                        }
                        float ratio_Start = Vector3.Distance(ArrLanePoints[j - 1, i], Pos_Intersection[j - 1]) / Dis_S2P[j - 1];
                        float ratio_End = Vector3.Distance(ArrLanePoints[j, i], Pos_Intersection[j - 1]) / Dis_E2P[j - 1];
                        LaneArr[i].bZPoints[j - 1].handle1 = ratio_Start * bZPoints[j - 1].handle1;
                        LaneArr[i].bZPoints[j].handle1 = ratio_End * bZPoints[j].handle1;
                    }
                }
            }
        }

        public void LinesUpdate()
        {
            for (int i = 0; i < LineArr.Count; i++)
            {
                LineArr[i].UpdateLines();
            }
        }
        int num = 0;
        private void LineCreat(int lineKind)
        {
            switch (lineKind)
            {
                case 0:
                    CreatLine("黄线", roads.Obj_Midline, midLines);
                    break;
                case 1:
                    CreatLane("交通线", RoadLanes,num%2==0);
                    num++;
                    break;
                case 2:
                    CreatLine("白线", roads.Obj_WhiteLine, roadLines);
                    break;
                case 3:
                    CreatLine("黑线", roads.Obj_BlackLine, roadEdges);
                    break;
                case 6:
                    break;
                default:
                    break;
            }
        }
        private void CreatLane(string name,Transform parent,bool isRe)
        {
            GameObject lineObj = new GameObject(name);
            lineObj.transform.SetParent(parent);
            Lane lane = lineObj.AddComponent<Lane>();
            lane.bZ = lineObj.AddComponent<BezierCurve>();
            lane.isReverse = isRe;
            lane.Init(bZPoints.Length);
            LaneArr.Add(lane);
        }
        private void LaneSameLanesSet()
        {
            foreach (Lane lane1 in LaneArr)
            {
                lane1.SameLanes = new List<Lane> { };
                foreach (Lane lane2 in LaneArr)
                {
                    if(lane2.isReverse==lane1.isReverse) lane1.SameLanes.Add(lane2);
                }
            }
        }
        private void CreatLine(string name, GameObject obj, Transform parent)
        {
            GameObject lineObj = new GameObject(name);
            lineObj.transform.SetParent(parent);
            Line line = lineObj.AddComponent<Line>();
            line.bZ = lineObj.AddComponent<BezierCurve>();
            line.LineObj = obj;
            line.Init(bZPoints.Length);
            LineArr.Add(line);
        }


        private Vector3 GetSamePos(Vector3 v1, Vector3 p1, Vector3 v2, Vector3 p2)
        {
            //Debug.Log("v1"+v1+"p1"+p1+ "v2" + v2 + "p2" + p2 );
            //ax+b=cx+d
            //x=(d-b)+/(a-c)
            float k1;//a
            float k2;//c
            float num1;//b
            float num2;//d
            if (v1 == v2||v1==-v2) return p1/2+p2/2;
            else if (v1.x == 0) //此时a为无限大，即垂直于x轴
            {
                num1 = p1.x;
                k2 = v2.z / v2.x;
                num2 = v2.z - k2 * v2.x;
                return new Vector3(p1.x, 0, k2 * p1.x + num2);
            }
            else if (v2.x == 0)//此时c为无限大，即垂直于x轴
            {
                k1 = v1.z / v1.x;
                num1 = v2.z - k1 * v1.x;
                return new Vector3(p2.x, 0, k1 * p2.x + num1);
            }
            else
            {
                k1 = v1.z / v1.x;
                k2 = v2.z / v2.x;
                num1 = p1.z - k1 * p1.x;
                num2 = p2.z - k2 * p2.x;
                float posX = (num2 - num1) / (k1 - k2);
                float posZ = k1 * posX + num1;
                float posZ1 = k2 * posX + num2;
                Debug.Log("normal"+ posZ+"-"+ posZ1);
                return new Vector3(posX, 0, posZ);
            }
        }
    }
}
