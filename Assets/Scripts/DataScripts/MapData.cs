using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Element
{

    public class Vec3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Vec3()
        {

        }
        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vec3(Vector3 v3)
        {
            X = v3.x;
            Y = v3.y;
            Z = v3.z;
        }

        public Vector3 GetVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }
    public class TransformData
    {
        public Vec3 V3Pos { get; set; }
        public Vec3 V3Rot { get; set; }
        public Vec3 V3Sca { get; set; }
        public TransformData()
        {

        }
        public TransformData(Vec3 pos, Vec3 rot, Vec3 sca)
        {
            V3Pos = pos;
            V3Rot = rot;
            V3Sca = sca;
        }
        public TransformData(Transform tran)
        {
            V3Pos = new Vec3(tran.position);
            V3Rot = new Vec3(tran.rotation.eulerAngles);
            V3Sca = new Vec3(tran.localScale);
        }
    }
    public class LaneData
    {
        public int LaneID { get; set; }
        public Vec3 PosStart { get; set; }
        public Vec3 PosEnd { get; set; }
        public float LaneLength { get; set; }
        public List<Vec3> List_pos { get; set; }
        public List<int> List_sameLanesID { get; set; }
        public LaneData() { }
        public LaneData(int id, Vector3 vStart, Vector3 vEnd, float length, List<Vector3> listPos, List<int> sameLanesID)
        {
            LaneID = id;
            PosStart = new Vec3(vStart);
            PosEnd = new Vec3(vEnd);
            LaneLength = length;
            List_pos = new List<Vec3>();
            foreach (Vector3 pos in listPos)
            {
                List_pos.Add(new Vec3(pos));
            }
            List_sameLanesID = sameLanesID;
        }
    }
    public class MapData
    {
        public List<LaneData> LanesData
        {
            get; set;
        }
        public MapData() { }
    }
}