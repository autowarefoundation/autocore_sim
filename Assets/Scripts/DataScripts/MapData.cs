using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public List<LaneData> LanesData
    {
        get; set;
    }
    public MapData() { }
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