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




using Packages.BezierCurveEditorPackage.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Edit
{
    public class Roads : MonoBehaviour
    {
        public enum RoadKind
        {
            D2L4_Normal,
            D2L2_Normal,
            D2L6_Normal
        }
        public Transform Trans_Start;
        public Transform Trans_End;
        public bool isEdit;
        public List<Lane> list_Lane;
        public GameObject Obj_WhiteLine;
        public GameObject Obj_BlackLine;
        public GameObject Obj_Lane;
        public GameObject Obj_Midline;
        public Transform Trans_RoadLanes;
        public Transform Trans_Connections;
        public Road[] roads;
        public int frame = 5;
        float tempTime = 0;
        public Dictionary<RoadKind, int[]> RoadDictionry = new Dictionary<RoadKind, int[]> {
            { RoadKind.D2L2_Normal, new int[] { 2, 2, 2 } },
            { RoadKind.D2L4_Normal, new int[] { 4,4,2} },
            { RoadKind.D2L6_Normal, new int[] { 6,6,2} }
        };
        void Update()
        {
            //roads = GetComponentsInChildren<Road>();
        }
        private void LineConnect()
        {
            for (int j = 0; j < roads.Length; j++)
            {
                Road road1 = roads[j];
                BezierPoint bZPointRoad1Start = road1.bZPoints[0];
                BezierPoint bZPointRoad1End = road1.bZPoints[road1.bZPoints.Length - 1];
                for (int i = 0; i < roads.Length; i++)
                {
                    Road road2 = roads[i];
                    BezierPoint bZPointRoad2Start = road2.bZPoints[0];
                    BezierPoint bZPointRoad2End = road2.bZPoints[road2.bZPoints.Length - 1];
                    float disS2E = Vector3.Distance(bZPointRoad1Start.position, bZPointRoad2End.position);
                    float disS2S = Vector3.Distance(bZPointRoad1Start.position, bZPointRoad2Start.position);
                    float disE2S = Vector3.Distance(bZPointRoad1End.position, bZPointRoad2Start.position);
                    float disE2E = Vector3.Distance(bZPointRoad1End.position, bZPointRoad2End.position);
                    if (disS2E < 5f && (bZPointRoad1Start.position != bZPointRoad2End.position || bZPointRoad1Start.handle1.normalized != bZPointRoad2End.handle1.normalized))
                    {
                        bZPointRoad1Start.position = bZPointRoad2End.position;
                        bZPointRoad1Start.handle1 = bZPointRoad2End.handle1;
                    }
                    if (disS2S < 5f && (bZPointRoad1Start.position != bZPointRoad2Start.position || bZPointRoad1Start.handle1.normalized != bZPointRoad2Start.handle1.normalized))
                    {
                        bZPointRoad1Start.position = bZPointRoad2Start.position;
                        bZPointRoad1Start.handle1 = -bZPointRoad2Start.handle1;
                    }
                    if (disE2S < 5f && (bZPointRoad1End.position != bZPointRoad2Start.position || bZPointRoad1End.handle1.normalized != bZPointRoad2Start.handle1.normalized))
                    {
                        bZPointRoad1End.position = bZPointRoad2Start.position;
                        bZPointRoad1End.handle1 = bZPointRoad2Start.handle1;
                    }
                    if (disE2E < 5f && (bZPointRoad1End.position != bZPointRoad2End.position || bZPointRoad1End.handle1.normalized != bZPointRoad2End.handle1.normalized))
                    {
                        bZPointRoad1End.position = bZPointRoad2End.position;
                        bZPointRoad1End.handle1 = -bZPointRoad2End.handle1;
                    }
                }
            }
            
        }
        public void SetLanes()
        {
            list_Lane = new List<Lane> { };
            Lane[] lanes = Trans_RoadLanes.GetComponentsInChildren<Lane>();
            Debug.Log("lanes.Length:" + lanes.Length);
            foreach (Lane item in lanes)
            {
                item.LaneCreat();
                list_Lane.Add(item);
            }
            Debug.Log("Lane Set OK");
        }
        public void LanesConnect()
        {
            while (Trans_Connections.childCount > 0)
            {
                DestroyImmediate(Trans_Connections.GetChild(0).gameObject);
            }
            list_Lane = new List<Lane> { };
            Lane[] lanes = Trans_RoadLanes.GetComponentsInChildren<Lane>();
            foreach (Lane lane1 in lanes)
            {
                Vector3 posStart = lane1.BZPEnd.position;
                foreach (Lane lane2 in lanes)
                {
                    Vector3 PosEnd = lane2.BZPStart.position;
                    if (Vector3.Distance(posStart, PosEnd) < 30)
                    {
                        CreatLane("Con" + lane1.name + "-" + lane2.name, Trans_Connections, posStart, PosEnd, (lane1.isReverse ? -1 : 1) * lane1.BZPEnd.handle1, (lane2.isReverse ? -1 : 1) * lane2.BZPStart.handle1);
                    }
                }
            }
        }

        public void SetRoads()
        {

        }

        private void CreatLane(string name, Transform parent,Vector3 posS,Vector3 posE,Vector3 handleS,Vector3 handleE)
        {
            GameObject lineObj = new GameObject(name);
            lineObj.transform.SetParent(parent);
            Lane lane = lineObj.AddComponent<Lane>();
            lane.bZ = lineObj.AddComponent<BezierCurve>();
            lane.isReverse = false;
            lane.Init(posS,posE,handleS.normalized*3,handleE.normalized * 3);
        }
    }
}
    
